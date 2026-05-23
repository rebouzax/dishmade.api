using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Dishes.Commands.UpdateDish;

public sealed class UpdateDishCommandHandler : IRequestHandler<UpdateDishCommand>
{
    private readonly IDishRepository _dishRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDishCommandHandler(
        IDishRepository dishRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _dishRepository = dishRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateDishCommand request,
        CancellationToken cancellationToken)
    {
        var dish = await _dishRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dish is null)
            throw new KeyNotFoundException("Prato não encontrado.");

        var categoryExists = await _categoryRepository.ExistsByIdAsync(
            request.CategoryId,
            cancellationToken);

        if (!categoryExists)
            throw new KeyNotFoundException("Categoria não encontrada.");

        var dishAlreadyExists = await _dishRepository.ExistsByNameAsync(
            request.Name,
            request.Id,
            cancellationToken);

        if (dishAlreadyExists)
            throw new InvalidOperationException("Já existe outro prato com esse nome.");

        dish.Update(
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}