using dishmade.application.Features.Public.ServiceRequests;
using MediatR;

namespace dishmade.application.Features.Public.ServiceRequests.Commands.StartServiceRequest;

public sealed record StartServiceRequestCommand(Guid Id)
    : IRequest<ServiceRequestResponse>;