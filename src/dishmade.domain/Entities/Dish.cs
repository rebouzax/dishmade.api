using dishmade.domain.Common;

namespace dishmade.domain.Entities;

public sealed class Dish : RestaurantScopedEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public bool IsAvailable { get; private set; } = true;
    public bool IsDeleted { get; private set; }

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    private Dish()
    {
    }

    public Dish(
        string name,
        string? description,
        decimal price,
        Guid categoryId,
        Guid restaurantId)
    {
        SetRestaurantId(restaurantId);
        ValidatePrice(price);

        Name = name.Trim();
        Description = description?.Trim();
        Price = price;
        CategoryId = categoryId;
    }

    public void Update(
        string name,
        string? description,
        decimal price,
        Guid categoryId)
    {
        ValidatePrice(price);

        Name = name.Trim();
        Description = description?.Trim();
        Price = price;
        CategoryId = categoryId;
        SetUpdatedAt();
    }

    public void SetAvailable()
    {
        IsAvailable = true;
        SetUpdatedAt();
    }

    public void SetUnavailable()
    {
        IsAvailable = false;
        SetUpdatedAt();
    }

    public void Delete()
    {
        IsDeleted = true;
        IsAvailable = false;
        SetUpdatedAt();
    }

    private static void ValidatePrice(decimal price)
    {
        if (price <= 0)
            throw new ArgumentException("O preço do prato deve ser maior que zero.", nameof(price));
    }
}