using dishmade.domain.Entities;

namespace dishmade.application.Abstractions.Repositories;

public interface IDishOptionRepository
{
    Task AddAsync(DishOption option, CancellationToken cancellationToken = default);

    Task<DishOption?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DishOption>> GetAvailableByIdsForDishAsync(
        IReadOnlyCollection<Guid> optionIds,
        Guid dishId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DishOption>> GetPublicAvailableByIdsForDishAsync(
        IReadOnlyCollection<Guid> optionIds,
        Guid dishId,
        Guid restaurantId,
        CancellationToken cancellationToken = default);
}