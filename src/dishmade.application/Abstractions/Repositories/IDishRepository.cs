using dishmade.application.Common.Pagination;
using dishmade.domain.Entities;

namespace dishmade.application.Abstractions.Repositories;

public interface IDishRepository
{
    Task AddAsync(Dish dish, CancellationToken cancellationToken = default);

    Task<Dish?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Dish>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<PagedResult<Dish>> GetPagedAsync(
        string? search,
        Guid? categoryId,
        bool? isAvailable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        string name,
        Guid? ignoredDishId = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Dish>> GetPublicByRestaurantIdAsync(
    Guid restaurantId,
    Guid? categoryId = null,
    CancellationToken cancellationToken = default);

    Task<Dish?> GetPublicAvailableByIdAsync(
        Guid dishId,
        Guid restaurantId,
        CancellationToken cancellationToken = default);
}