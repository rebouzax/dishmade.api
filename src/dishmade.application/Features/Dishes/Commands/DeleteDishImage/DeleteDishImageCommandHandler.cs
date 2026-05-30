using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Dishes.Commands.DeleteDishImage;

public sealed class DeleteDishImageCommandHandler : IRequestHandler<DeleteDishImageCommand>
{
    private readonly IDishRepository _dishRepository;
    private readonly IDishImageRepository _dishImageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDishImageCommandHandler(
        IDishRepository dishRepository,
        IDishImageRepository dishImageRepository,
        IUnitOfWork unitOfWork)
    {
        _dishRepository = dishRepository;
        _dishImageRepository = dishImageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeleteDishImageCommand request,
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

        _dishImageRepository.Remove(image);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}