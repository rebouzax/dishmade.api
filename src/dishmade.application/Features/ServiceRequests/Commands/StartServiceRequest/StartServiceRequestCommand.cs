using MediatR;

namespace dishmade.application.Features.ServiceRequests.Commands.StartServiceRequest;

public sealed record StartServiceRequestCommand(Guid Id)
    : IRequest<ServiceRequestResponse>;