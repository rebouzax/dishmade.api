using dishmade.domain.Entities;

namespace dishmade.application.Abstractions.Repositories;

public interface IDishOptionGroupRepository
{
    Task AddAsync(DishOptionGroup group, CancellationToken cancellationToken = default);

    Task<DishOptionGroup?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DishOptionGroup>> GetByDishIdAsync(
        Guid dishId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DishOptionGroup>> GetPublicByDishIdAsync(
        Guid dishId,
        Guid restaurantId,
        CancellationToken cancellationToken = default);
}