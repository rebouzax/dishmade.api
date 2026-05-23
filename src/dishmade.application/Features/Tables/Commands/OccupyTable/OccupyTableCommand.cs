using MediatR;

namespace dishmade.application.Features.Tables.Commands.OccupyTable;

public sealed record OccupyTableCommand(Guid Id) : IRequest;