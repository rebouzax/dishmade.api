using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Pagination;
using dishmade.application.Features.Tables.Queries;
using MediatR;

namespace dishmade.application.Features.Tables.Queries.GetTables;

public sealed class GetTablesQueryHandler
    : IRequestHandler<GetTablesQuery, PagedResponse<TableResponse>>
{
    private readonly IRestaurantTableRepository _tableRepository;

    public GetTablesQueryHandler(IRestaurantTableRepository tableRepository)
    {
        _tableRepository = tableRepository;
    }

    public async Task<PagedResponse<TableResponse>> Handle(
        GetTablesQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = PaginationHelper.NormalizePageNumber(request.PageNumber);
        var pageSize = PaginationHelper.NormalizePageSize(request.PageSize);

        var result = await _tableRepository.GetPagedAsync(
            request.Number,
            request.IsOccupied,
            pageNumber,
            pageSize,
            cancellationToken);

        var tables = result.Items
            .Select(table => new TableResponse(
                table.Id,
                table.Number,
                table.IsOccupied,
                table.CreatedAt,
                table.UpdatedAt))
            .ToList();

        return new PagedResponse<TableResponse>(
            tables,
            pageNumber,
            pageSize,
            result.TotalCount);
    }
}