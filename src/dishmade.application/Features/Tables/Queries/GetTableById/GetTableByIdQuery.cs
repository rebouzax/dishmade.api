using dishmade.application.Features.Tables.Queries;
using MediatR;

namespace dishmade.application.Features.Tables.Queries.GetTableById;

public sealed record GetTableByIdQuery(Guid Id) : IRequest<TableResponse>;