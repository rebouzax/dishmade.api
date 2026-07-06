using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Pagination;
using MediatR;

namespace dishmade.application.Features.ServiceRequests.Queries.GetServiceRequests;

public sealed class GetServiceRequestsQueryHandler
    : IRequestHandler<GetServiceRequestsQuery, PagedResponse<ServiceRequestResponse>>
{
    private readonly IServiceRequestRepository _serviceRequestRepository;

    public GetServiceRequestsQueryHandler(IServiceRequestRepository serviceRequestRepository)
    {
        _serviceRequestRepository = serviceRequestRepository;
    }

    public async Task<PagedResponse<ServiceRequestResponse>> Handle(
        GetServiceRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = PaginationHelper.NormalizePageNumber(request.PageNumber);
        var pageSize = PaginationHelper.NormalizePageSize(request.PageSize);

        var result = await _serviceRequestRepository.GetPagedAsync(
            request.Status,
            request.Type,
            request.TableId,
            pageNumber,
            pageSize,
            cancellationToken);

        var items = result.Items
            .Select(ServiceRequestMapper.ToResponse)
            .ToList();

        return new PagedResponse<ServiceRequestResponse>(
            items,
            pageNumber,
            pageSize,
            result.TotalCount);
    }
}