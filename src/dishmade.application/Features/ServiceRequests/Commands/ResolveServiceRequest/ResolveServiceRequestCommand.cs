using MediatR;

namespace dishmade.application.Features.ServiceRequests.Commands.ResolveServiceRequest;

public sealed record ResolveServiceRequestCommand(Guid Id)
    : IRequest<ServiceRequestResponse>;