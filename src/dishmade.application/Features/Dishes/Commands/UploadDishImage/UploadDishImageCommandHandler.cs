using dishmade.application.Abstractions.Auth;
using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.domain.Entities;
using MediatR;

namespace dishmade.application.Features.Dishes.Commands.UploadDishImage;

public sealed class UploadDishImageCommandHandler : IRequestHandler<UploadDishImageCommand>
{
    private readonly IDishRepository _dishRepository;
    private readonly IDishImageRepository _dishImageRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public UploadDishImageCommandHandler(
        IDishRepository dishRepository,
        IDishImageRepository dishImageRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _dishRepository = dishRepository;
        _dishImageRepository = dishImageRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UploadDishImageCommand request,
        CancellationToken cancellationToken)
    {
        var restaurantId = _currentUserService.GetRequiredRestaurantId();

        var dish = await _dishRepository.GetByIdAsync(request.DishId, cancellationToken);

        if (dish is null)
            throw new KeyNotFoundException("Prato não encontrado.");

        var existingImage = await _dishImageRepository.GetByDishIdAsync(
            request.DishId,
            cancellationToken);

        if (existingImage is null)
        {
            var image = new DishImage(
                request.DishId,
                restaurantId,
                request.FileName,
                request.ContentType,
                request.SizeInBytes,
                request.Data);

            await _dishImageRepository.AddAsync(image, cancellationToken);
        }
        else
        {
            existingImage.Update(
                request.FileName,
                request.ContentType,
                request.SizeInBytes,
                request.Data);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}