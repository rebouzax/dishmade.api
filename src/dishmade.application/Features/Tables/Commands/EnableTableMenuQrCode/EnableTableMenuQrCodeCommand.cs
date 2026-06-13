using dishmade.application.Features.Tables.MenuQrCode;
using MediatR;

namespace dishmade.application.Features.Tables.Commands.EnableTableMenuQrCode;

public sealed record EnableTableMenuQrCodeCommand(Guid TableId)
    : IRequest<TableMenuQrCodeResponse>;