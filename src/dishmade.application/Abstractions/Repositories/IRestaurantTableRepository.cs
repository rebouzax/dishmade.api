using dishmade.application.Common.Pagination;
using dishmade.domain.Entities;

namespace dishmade.application.Abstractions.Repositories;

public interface IRestaurantTableRepository
{
    Task AddAsync(RestaurantTable table, CancellationToken cancellationToken = default);

    Task<RestaurantTable?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RestaurantTable>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<PagedResult<RestaurantTable>> GetPagedAsync(
        int? number,
        bool? isOccupied,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNumberAsync(
        int number,
        Guid? ignoredTableId = null,
        CancellationToken cancellationToken = default);
}