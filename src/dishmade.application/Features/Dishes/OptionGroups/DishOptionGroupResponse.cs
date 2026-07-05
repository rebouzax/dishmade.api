namespace dishmade.application.Features.Dishes.OptionGroups;

public sealed record DishOptionGroupResponse(
    Guid Id,
    Guid DishId,
    string Name,
    bool IsRequired,
    int MinSelection,
    int MaxSelection,
    bool IsActive,
    IReadOnlyList<DishOptionResponse> Options
);

public sealed record DishOptionResponse(
    Guid Id,
    Guid OptionGroupId,
    string Name,
    decimal AdditionalPrice,
    bool IsAvailable
);