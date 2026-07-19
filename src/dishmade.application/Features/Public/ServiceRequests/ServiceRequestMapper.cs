using dishmade.domain.Entities;

namespace dishmade.application.Features.Public.ServiceRequests;

public static class ServiceRequestMapper
{
    public static ServiceRequestResponse ToResponse(ServiceRequest request)
    {
        return new ServiceRequestResponse(
            request.Id,
            request.RestaurantId,
            request.TableId,
            request.Table.Number,
            request.Type,
            request.Status,
            request.Message,
            request.CreatedAt,
            request.StartedAt,
            request.ResolvedAt,
            request.CanceledAt,
            request.UpdatedAt);
    }
}