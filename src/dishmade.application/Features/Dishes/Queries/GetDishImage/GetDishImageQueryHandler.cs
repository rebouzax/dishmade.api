using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Dishes.Queries.GetDishImage;

public sealed class GetDishImageQueryHandler : IRequestHandler<GetDishImageQuery, DishImageResponse>
{
    private readonly IDishRepository _dishRepository;
    private readonly IDishImageRepository _dishImageRepository;

    public GetDishImageQueryHandler(
        IDishRepository dishRepository,
        IDishImageRepository dishImageRepository)
    {
        _dishRepository = dishRepository;
        _dishImageRepository = dishImageRepository;
    }

    public async Task<DishImageResponse> Handle(
        GetDishImageQuery request,
        CancellationToken cancellationToken)
    {
        var dish = await _dishRepository.GetByIdAsync(request.DishId, cancellationToken);

        if (dish is null)
            throw new KeyNotFoundException("Prato não encontrado.");

        var image = await _dishImageRepository.GetByDishIdAsync(
            request.DishId,
            cancellationToken);

        if (image is null)
            throw new KeyNotFoundException("Imagem do prato não encontrada.");

        return new DishImageResponse(
            image.FileName,
            image.ContentType,
            image.SizeInBytes,
            image.Data);
    }
}