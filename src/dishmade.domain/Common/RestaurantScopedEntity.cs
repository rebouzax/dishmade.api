namespace dishmade.domain.Common;

public abstract class RestaurantScopedEntity : BaseEntity
{
    public Guid RestaurantId { get; private set; }

    protected void SetRestaurantId(Guid restaurantId)
    {
        if (restaurantId == Guid.Empty)
            throw new ArgumentException("O restaurante é obrigatório.", nameof(restaurantId));

        RestaurantId = restaurantId;
    }
}