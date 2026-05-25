using dishmade.domain.Common;

namespace dishmade.domain.Entities;

public sealed class Restaurant : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Document { get; private set; }
    public bool IsActive { get; private set; } = true;

    public ICollection<AppUser> Users { get; private set; } = [];

    private Restaurant()
    {
    }

    public Restaurant(string name, string? document)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do restaurante é obrigatório.", nameof(name));

        Name = name.Trim();
        Document = document?.Trim();
    }

    public void Update(string name, string? document)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do restaurante é obrigatório.", nameof(name));

        Name = name.Trim();
        Document = document?.Trim();
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