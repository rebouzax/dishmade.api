using dishmade.application.Common.Pagination;
using dishmade.domain.Enums;
using MediatR;

namespace dishmade.application.Features.ServiceRequests.Queries.GetServiceRequests;

public sealed record GetServiceRequestsQuery(
    ServiceRequestStatus? Status,
    ServiceRequestType? Type,
    Guid? TableId,
    int PageNumber,
    int PageSize
) : IRequest<PagedResponse<ServiceRequestResponse>>;