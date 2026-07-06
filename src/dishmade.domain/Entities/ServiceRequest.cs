using dishmade.domain.Common;
using dishmade.domain.Enums;

namespace dishmade.domain.Entities;

public sealed class ServiceRequest : RestaurantScopedEntity
{
    public Guid TableId { get; private set; }
    public RestaurantTable Table { get; private set; } = null!;

    public ServiceRequestType Type { get; private set; }
    public ServiceRequestStatus Status { get; private set; } = ServiceRequestStatus.Pending;

    public string? Message { get; private set; }

    public DateTime? StartedAt { get; private set; }
    public DateTime? ResolvedAt { get; private set; }
    public DateTime? CanceledAt { get; private set; }

    private ServiceRequest()
    {
    }

    public ServiceRequest(
        Guid restaurantId,
        Guid tableId,
        ServiceRequestType type,
        string? message)
    {
        SetRestaurantId(restaurantId);

        TableId = tableId;
        Type = type;
        Message = NormalizeMessage(message);
    }

    public void Start()
    {
        EnsureCanBeChanged();

        if (Status != ServiceRequestStatus.Pending)
            throw new InvalidOperationException("Somente solicitações pendentes podem ser iniciadas.");

        Status = ServiceRequestStatus.InProgress;
        StartedAt = DateTime.UtcNow;

        SetUpdatedAt();
    }

    public void Resolve()
    {
        EnsureCanBeChanged();

        if (Status != ServiceRequestStatus.Pending &&
            Status != ServiceRequestStatus.InProgress)
        {
            throw new InvalidOperationException("Somente solicitações pendentes ou em andamento podem ser resolvidas.");
        }

        Status = ServiceRequestStatus.Resolved;
        ResolvedAt = DateTime.UtcNow;

        SetUpdatedAt();
    }

    public void Cancel()
    {
        EnsureCanBeChanged();

        Status = ServiceRequestStatus.Canceled;
        CanceledAt = DateTime.UtcNow;

        SetUpdatedAt();
    }

    private void EnsureCanBeChanged()
    {
        if (Status == ServiceRequestStatus.Resolved)
            throw new InvalidOperationException("Solicitações resolvidas não podem ser alteradas.");

        if (Status == ServiceRequestStatus.Canceled)
            throw new InvalidOperationException("Solicitações canceladas não podem ser alteradas.");
    }

    private static string? NormalizeMessage(string? message)
    {
        return string.IsNullOrWhiteSpace(message)
            ? null
            : message.Trim();
    }
}