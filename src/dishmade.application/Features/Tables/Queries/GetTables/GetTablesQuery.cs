using dishmade.application.Common.Pagination;
using MediatR;

namespace dishmade.application.Features.Tables.Queries.GetTables;

public sealed record GetTablesQuery(
    int? Number,
    bool? IsOccupied,
    int PageNumber,
    int PageSize
) : IRequest<PagedResponse<TableResponse>>;