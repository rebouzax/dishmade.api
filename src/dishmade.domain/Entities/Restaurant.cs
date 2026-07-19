using dishmade.domain.Common;

namespace dishmade.domain.Entities;

public sealed class Restaurant : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? Document { get; private set; }
    public bool IsActive { get; private set; } = true;

    public decimal DefaultServiceFeePercentage { get; private set; }
    public bool AcceptsQrCodeOrders { get; private set; } = true;
    public bool AcceptsWaiterCall { get; private set; } = true;

    public ICollection<AppUser> Users { get; private set; } = [];

    private Restaurant()
    {
    }

    public Restaurant(string name, string? document, string slug)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do restaurante é obrigatório.", nameof(name));

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("O slug do restaurante é obrigatório.", nameof(slug));

        Name = name.Trim();
        Document = document?.Trim();
        Slug = slug.Trim().ToLowerInvariant();

        DefaultServiceFeePercentage = 10;
        AcceptsQrCodeOrders = true;
        AcceptsWaiterCall = true;
    }

    public void Update(string name, string? document, string slug)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do restaurante é obrigatório.", nameof(name));

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("O slug do restaurante é obrigatório.", nameof(slug));

        Name = name.Trim();
        Document = document?.Trim();
        Slug = slug.Trim().ToLowerInvariant();

        SetUpdatedAt();
    }

    public void UpdateOperationalSettings(
        decimal defaultServiceFeePercentage,
        bool acceptsQrCodeOrders,
        bool acceptsWaiterCall)
    {
        if (defaultServiceFeePercentage < 0)
            throw new ArgumentException("A taxa de serviço padrão não pode ser negativa.", nameof(defaultServiceFeePercentage));

        if (defaultServiceFeePercentage > 100)
            throw new ArgumentException("A taxa de serviço padrão não pode ser maior que 100%.", nameof(defaultServiceFeePercentage));

        DefaultServiceFeePercentage = defaultServiceFeePercentage;
        AcceptsQrCodeOrders = acceptsQrCodeOrders;
        AcceptsWaiterCall = acceptsWaiterCall;

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