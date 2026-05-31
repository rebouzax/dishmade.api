using dishmade.domain.Common;

namespace dishmade.domain.Entities;

public sealed class Restaurant : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? Document { get; private set; }
    public bool IsActive { get; private set; } = true;

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