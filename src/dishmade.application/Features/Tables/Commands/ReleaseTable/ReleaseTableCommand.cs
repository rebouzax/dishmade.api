using MediatR;

namespace dishmade.application.Features.Tables.Commands.ReleaseTable;

public sealed record ReleaseTableCommand(Guid Id) : IRequest;