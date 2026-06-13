namespace dishmade.application.Features.Tables.Queries;

public sealed record TableResponse(
    Guid Id,
    int Number,
    bool IsOccupied,
    bool IsMenuQrCodeEnabled,
    DateTime? MenuQrCodeEnabledAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);