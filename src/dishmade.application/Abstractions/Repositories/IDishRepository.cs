using dishmade.domain.Entities;

namespace dishmade.application.Abstractions.Repositories;

public interface IDishRepository
{
    Task AddAsync(Dish dish, CancellationToken cancellationToken = default);

    Task<Dish?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Dish>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        string name,
        Guid? ignoredDishId = null,
        CancellationToken cancellationToken = default);
}