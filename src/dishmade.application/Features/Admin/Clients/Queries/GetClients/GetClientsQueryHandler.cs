using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Pagination;
using MediatR;

namespace dishmade.application.Features.Admin.Clients.Queries.GetClients;

public sealed class GetClientsQueryHandler
    : IRequestHandler<GetClientsQuery, PagedResponse<ClientResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetClientsQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PagedResponse<ClientResponse>> Handle(
        GetClientsQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = PaginationHelper.NormalizePageNumber(request.PageNumber);
        var pageSize = PaginationHelper.NormalizePageSize(request.PageSize);

        var result = await _userRepository.GetClientsPagedAsync(
            request.Search,
            request.IsActive,
            pageNumber,
            pageSize,
            cancellationToken);

        var clients = result.Items
            .Select(user => new ClientResponse(
                user.Id,
                user.Name,
                user.Email,
                user.IsActive,
                user.RestaurantId,
                user.Restaurant?.Name,
                user.Restaurant?.Slug,
                user.Restaurant?.Document,
                user.Restaurant?.IsActive,
                user.CreatedAt,
                user.UpdatedAt))
            .ToList();

        return new PagedResponse<ClientResponse>(
            clients,
            pageNumber,
            pageSize,
            result.TotalCount);
    }
}