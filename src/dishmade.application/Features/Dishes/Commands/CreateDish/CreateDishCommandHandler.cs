using dishmade.application.Abstractions.Auth;
using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.domain.Entities;
using MediatR;

namespace dishmade.application.Features.Dishes.Commands.CreateDish;

public sealed class CreateDishCommandHandler : IRequestHandler<CreateDishCommand, Guid>
{
    private readonly IDishRepository _dishRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateDishCommandHandler(
        IDishRepository dishRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _dishRepository = dishRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateDishCommand request,
        CancellationToken cancellationToken)
    {
        var categoryExists = await _categoryRepository.ExistsByIdAsync(
            request.CategoryId,
            cancellationToken);

        if (!categoryExists)
            throw new KeyNotFoundException("Categoria não encontrada.");

        var dishAlreadyExists = await _dishRepository.ExistsByNameAsync(
            request.Name,
            cancellationToken: cancellationToken);

        if (dishAlreadyExists)
            throw new InvalidOperationException("Já existe um prato com esse nome.");

        var restaurantId = _currentUserService.GetRequiredRestaurantId();

        var dish = new Dish(
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId,
            restaurantId);

        await _dishRepository.AddAsync(dish, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return dish.Id;
    }
}