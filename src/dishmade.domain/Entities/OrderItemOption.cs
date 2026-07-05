using dishmade.domain.Common;

namespace dishmade.domain.Entities;

public sealed class OrderItemOption : BaseEntity
{
    public Guid OrderItemId { get; private set; }
    public OrderItem OrderItem { get; private set; } = null!;

    public Guid DishOptionId { get; private set; }
    public DishOption DishOption { get; private set; } = null!;

    public string OptionName { get; private set; } = string.Empty;
    public decimal AdditionalPrice { get; private set; }

    private OrderItemOption()
    {
    }

    public OrderItemOption(
        Guid orderItemId,
        Guid dishOptionId,
        string optionName,
        decimal additionalPrice)
    {
        if (string.IsNullOrWhiteSpace(optionName))
            throw new ArgumentException("O nome da opção é obrigatório.", nameof(optionName));

        if (additionalPrice < 0)
            throw new ArgumentException("O preço adicional não pode ser negativo.", nameof(additionalPrice));

        OrderItemId = orderItemId;
        DishOptionId = dishOptionId;
        OptionName = optionName.Trim();
        AdditionalPrice = additionalPrice;
    }
}