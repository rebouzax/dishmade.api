using dishmade.domain.Common;

namespace dishmade.domain.Entities;

public sealed class DishOptionGroup : RestaurantScopedEntity
{
    public Guid DishId { get; private set; }
    public Dish Dish { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public bool IsRequired { get; private set; }
    public int MinSelection { get; private set; }
    public int MaxSelection { get; private set; }

    public bool IsActive { get; private set; } = true;
    public bool IsDeleted { get; private set; }

    public ICollection<DishOption> Options { get; private set; } = [];

    private DishOptionGroup()
    {
    }

    public DishOptionGroup(
        Guid dishId,
        Guid restaurantId,
        string name,
        bool isRequired,
        int minSelection,
        int maxSelection)
    {
        SetRestaurantId(restaurantId);
        ValidateSelectionRules(isRequired, minSelection, maxSelection);

        DishId = dishId;
        Name = NormalizeName(name);
        IsRequired = isRequired;
        MinSelection = minSelection;
        MaxSelection = maxSelection;
    }

    public void Update(
        string name,
        bool isRequired,
        int minSelection,
        int maxSelection)
    {
        ValidateSelectionRules(isRequired, minSelection, maxSelection);

        Name = NormalizeName(name);
        IsRequired = isRequired;
        MinSelection = minSelection;
        MaxSelection = maxSelection;

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

    public void Delete()
    {
        IsDeleted = true;
        IsActive = false;
        SetUpdatedAt();
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do grupo de opções é obrigatório.", nameof(name));

        return name.Trim();
    }

    private static void ValidateSelectionRules(
        bool isRequired,
        int minSelection,
        int maxSelection)
    {
        if (minSelection < 0)
            throw new ArgumentException("A seleção mínima não pode ser negativa.", nameof(minSelection));

        if (maxSelection <= 0)
            throw new ArgumentException("A seleção máxima deve ser maior que zero.", nameof(maxSelection));

        if (minSelection > maxSelection)
            throw new ArgumentException("A seleção mínima não pode ser maior que a seleção máxima.");

        if (isRequired && minSelection <= 0)
            throw new ArgumentException("Grupos obrigatórios devem ter seleção mínima maior que zero.");
    }
}