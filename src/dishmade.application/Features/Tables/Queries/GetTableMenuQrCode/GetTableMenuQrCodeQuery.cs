using dishmade.application.Features.Tables.MenuQrCode;
using MediatR;

namespace dishmade.application.Features.Tables.Queries.GetTableMenuQrCode;

public sealed record GetTableMenuQrCodeQuery(Guid TableId)
    : IRequest<TableMenuQrCodeResponse>;