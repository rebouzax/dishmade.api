using dishmade.domain.Common;

namespace dishmade.domain.Entities;

public sealed class DishOption : RestaurantScopedEntity
{
    public Guid OptionGroupId { get; private set; }
    public DishOptionGroup OptionGroup { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;
    public decimal AdditionalPrice { get; private set; }

    public bool IsAvailable { get; private set; } = true;
    public bool IsDeleted { get; private set; }

    private DishOption()
    {
    }

    public DishOption(
        Guid optionGroupId,
        Guid restaurantId,
        string name,
        decimal additionalPrice)
    {
        SetRestaurantId(restaurantId);
        ValidateAdditionalPrice(additionalPrice);

        OptionGroupId = optionGroupId;
        Name = NormalizeName(name);
        AdditionalPrice = additionalPrice;
    }

    public void Update(
        string name,
        decimal additionalPrice)
    {
        ValidateAdditionalPrice(additionalPrice);

        Name = NormalizeName(name);
        AdditionalPrice = additionalPrice;

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

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da opção é obrigatório.", nameof(name));

        return name.Trim();
    }

    private static void ValidateAdditionalPrice(decimal additionalPrice)
    {
        if (additionalPrice < 0)
            throw new ArgumentException("O preço adicional não pode ser negativo.", nameof(additionalPrice));
    }
}