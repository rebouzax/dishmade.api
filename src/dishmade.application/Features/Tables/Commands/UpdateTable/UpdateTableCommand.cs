using MediatR;

namespace dishmade.application.Features.Tables.Commands.UpdateTable;

public sealed record UpdateTableCommand(
    Guid Id,
    int Number
) : IRequest;