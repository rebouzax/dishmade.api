using MediatR;

namespace dishmade.application.Features.Tables.Commands.DisableTableMenuQrCode;

public sealed record DisableTableMenuQrCodeCommand(Guid TableId) : IRequest;