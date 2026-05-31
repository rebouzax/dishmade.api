using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Dishes.Queries.GetDishImage;
using MediatR;

namespace dishmade.application.Features.Public.Menu.Queries.GetPublicDishImage;

public sealed class GetPublicDishImageQueryHandler
    : IRequestHandler<GetPublicDishImageQuery, DishImageResponse>
{
    private readonly IDishImageRepository _dishImageRepository;

    public GetPublicDishImageQueryHandler(IDishImageRepository dishImageRepository)
    {
        _dishImageRepository = dishImageRepository;
    }

    public async Task<DishImageResponse> Handle(
        GetPublicDishImageQuery request,
        CancellationToken cancellationToken)
    {
        var image = await _dishImageRepository.GetPublicByDishIdAsync(
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