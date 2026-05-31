using dishmade.application.Common.Pagination;
using dishmade.domain.Entities;
using dishmade.domain.Enums;

namespace dishmade.application.Abstractions.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken = default);

    Task AddItemAsync(OrderItem item, CancellationToken cancellationToken = default);

    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<PagedResult<Order>> GetPagedAsync(
        OrderStatus? status,
        Guid? tableId,
        DateTime? startDate,
        DateTime? endDate,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Order>> GetDeliveredOrdersAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);

    Task<PagedResult<Order>> GetDeliveredOrdersPagedAsync(
        DateTime? startDate,
        DateTime? endDate,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Order?> GetPublicByIdAndAccessCodeAsync(
    Guid orderId,
    string accessCode,
    CancellationToken cancellationToken = default);
}