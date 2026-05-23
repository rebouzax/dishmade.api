namespace dishmade.application.Features.Dishes.Queries;

public sealed record DishResponse(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    bool IsAvailable,
    Guid CategoryId,
    string CategoryName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);