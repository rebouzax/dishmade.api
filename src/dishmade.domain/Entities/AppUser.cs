using dishmade.domain.Common;
using dishmade.domain.Constants;

namespace dishmade.domain.Entities;

public sealed class AppUser : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;

    public Guid? RestaurantId { get; private set; }
    public Restaurant? Restaurant { get; private set; }

    private AppUser()
    {
    }

    private AppUser(
        string name,
        string email,
        string role,
        Guid? restaurantId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do usuário é obrigatório.", nameof(name));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O e-mail do usuário é obrigatório.", nameof(email));

        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        Role = role;
        RestaurantId = restaurantId;
    }

    public static AppUser CreatePlatformAdmin(string name, string email)
    {
        return new AppUser(
            name,
            email,
            Roles.PlatformAdmin,
            restaurantId: null);
    }

    public static AppUser CreateClient(string name, string email, Guid restaurantId)
    {
        return new AppUser(
            name,
            email,
            Roles.Client,
            restaurantId);
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("O hash da senha é obrigatório.", nameof(passwordHash));

        PasswordHash = passwordHash;
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