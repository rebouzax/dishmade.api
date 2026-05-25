using dishmade.domain.Common;

namespace dishmade.domain.Entities;

public sealed class RestaurantTable : RestaurantScopedEntity
{
    public int Number { get; private set; }
    public bool IsOccupied { get; private set; }
    public bool IsDeleted { get; private set; }

    private RestaurantTable()
    {
    }

    public RestaurantTable(int number, Guid restaurantId)
    {
        SetRestaurantId(restaurantId);
        ValidateNumber(number);

        Number = number;
        IsOccupied = false;
        IsDeleted = false;
    }

    public void UpdateNumber(int number)
    {
        ValidateNumber(number);

        Number = number;
        SetUpdatedAt();
    }

    public void Occupy()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Não é possível ocupar uma mesa removida.");

        if (IsOccupied)
            throw new InvalidOperationException("A mesa já está ocupada.");

        IsOccupied = true;
        SetUpdatedAt();
    }

    public void Release()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Não é possível liberar uma mesa removida.");

        if (!IsOccupied)
            throw new InvalidOperationException("A mesa já está livre.");

        IsOccupied = false;
        SetUpdatedAt();
    }

    public void Delete()
    {
        if (IsOccupied)
            throw new InvalidOperationException("Não é possível remover uma mesa ocupada.");

        IsDeleted = true;
        SetUpdatedAt();
    }

    private static void ValidateNumber(int number)
    {
        if (number <= 0)
            throw new ArgumentException("O número da mesa deve ser maior que zero.", nameof(number));
    }
}