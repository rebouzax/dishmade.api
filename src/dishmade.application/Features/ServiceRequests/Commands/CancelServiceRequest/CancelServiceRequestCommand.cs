using MediatR;

namespace dishmade.application.Features.ServiceRequests.Commands.CancelServiceRequest;

public sealed record CancelServiceRequestCommand(Guid Id)
    : IRequest<ServiceRequestResponse>;