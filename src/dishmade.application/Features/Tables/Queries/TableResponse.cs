namespace dishmade.application.Features.Tables.Queries;

public sealed record TableResponse(
    Guid Id,
    int Number,
    bool IsOccupied,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);