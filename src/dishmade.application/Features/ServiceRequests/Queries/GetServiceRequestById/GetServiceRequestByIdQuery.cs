using MediatR;

namespace dishmade.application.Features.ServiceRequests.Queries.GetServiceRequestById;

public sealed record GetServiceRequestByIdQuery(Guid Id)
    : IRequest<ServiceRequestResponse>;