using MediatR;

namespace dishmade.application.Features.Tables.Queries.GetTables;

public sealed record GetTablesQuery : IRequest<IReadOnlyList<TableResponse>>;