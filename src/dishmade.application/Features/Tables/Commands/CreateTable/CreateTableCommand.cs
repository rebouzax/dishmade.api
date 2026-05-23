using MediatR;

namespace dishmade.application.Features.Tables.Commands.CreateTable;

public sealed record CreateTableCommand(int Number) : IRequest<Guid>;