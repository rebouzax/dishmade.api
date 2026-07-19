using dishmade.domain.Enums;
using MediatR;

namespace dishmade.application.Features.Public.ServiceRequests.Commands.CreatePublicServiceRequest;

public sealed record CreatePublicServiceRequestCommand(
    string RestaurantSlug,
    int TableNumber,
    ServiceRequestType Type,
    string? Message
) : IRequest<ServiceRequestResponse>;