using dishmade.application.Common.Pagination;
using MediatR;

namespace dishmade.application.Features.Admin.Clients.Queries.GetClients;

public sealed record GetClientsQuery(
    string? Search,
    bool? IsActive,
    int PageNumber,
    int PageSize
) : IRequest<PagedResponse<ClientResponse>>;