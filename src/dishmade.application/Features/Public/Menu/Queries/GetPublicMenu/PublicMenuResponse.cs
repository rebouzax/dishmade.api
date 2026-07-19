namespace dishmade.application.Features.Public.Menu.Queries.GetPublicMenu;

public sealed record PublicMenuResponse(
    Guid RestaurantId,
    string RestaurantName,
    string Slug,
    string MenuUrl,
    string QrCodeUrl,
    decimal DefaultServiceFeePercentage,
    bool AcceptsQrCodeOrders,
    bool AcceptsWaiterCall,
    IReadOnlyList<PublicCategoryResponse> Categories
);

public sealed record PublicCategoryResponse(
    Guid Id,
    string Name,
    string? Description,
    IReadOnlyList<PublicDishResponse> Dishes
);

public sealed record PublicDishResponse(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId,
    string CategoryName,
    string ImageUrl,
    IReadOnlyList<PublicDishOptionGroupResponse> OptionGroups
);

public sealed record PublicDishOptionGroupResponse(
    Guid Id,
    string Name,
    bool IsRequired,
    int MinSelection,
    int MaxSelection,
    IReadOnlyList<PublicDishOptionResponse> Options
);

public sealed record PublicDishOptionResponse(
    Guid Id,
    string Name,
    decimal AdditionalPrice
);