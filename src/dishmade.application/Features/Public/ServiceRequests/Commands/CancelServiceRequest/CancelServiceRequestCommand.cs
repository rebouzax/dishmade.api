using dishmade.application.Features.Public.ServiceRequests;
using MediatR;

namespace dishmade.application.Features.Public.ServiceRequests.Commands.CancelServiceRequest;

public sealed record CancelServiceRequestCommand(Guid Id)
    : IRequest<ServiceRequestResponse>;