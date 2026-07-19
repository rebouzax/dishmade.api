using dishmade.application.Features.Public.ServiceRequests;
using MediatR;

namespace dishmade.application.Features.Public.ServiceRequests.Commands.ResolveServiceRequest;

public sealed record ResolveServiceRequestCommand(Guid Id)
    : IRequest<ServiceRequestResponse>;