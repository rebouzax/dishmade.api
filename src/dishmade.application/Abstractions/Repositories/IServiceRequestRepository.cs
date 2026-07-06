using dishmade.application.Common.Pagination;
using dishmade.domain.Entities;
using dishmade.domain.Enums;

namespace dishmade.application.Abstractions.Repositories;

public interface IServiceRequestRepository
{
    Task AddAsync(ServiceRequest request, CancellationToken cancellationToken = default);

    Task<ServiceRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PagedResult<ServiceRequest>> GetPagedAsync(
        ServiceRequestStatus? status,
        ServiceRequestType? type,
        Guid? tableId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}