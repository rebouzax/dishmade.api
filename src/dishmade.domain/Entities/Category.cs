using dishmade.domain.Common;

namespace dishmade.domain.Entities;

public sealed class Category : RestaurantScopedEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;

    public ICollection<Dish> Dishes { get; private set; } = [];

    private Category()
    {
    }

    public Category(
        string name,
        string? description,
        Guid restaurantId)
    {
        SetRestaurantId(restaurantId);

        Name = name.Trim();
        Description = description?.Trim();
    }

    public void Update(string name, string? description)
    {
        Name = name.Trim();
        Description = description?.Trim();
        SetUpdatedAt();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }
}