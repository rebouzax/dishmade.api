using MediatR;

namespace dishmade.application.Features.Tables.Commands.DeleteTable;

public sealed record DeleteTableCommand(Guid Id) : IRequest;