using dishmade.application.Features.Public.ServiceRequests;
using MediatR;

namespace dishmade.application.Features.Public.ServiceRequests.Queries.GetServiceRequestById;

public sealed record GetServiceRequestByIdQuery(Guid Id)
    : IRequest<ServiceRequestResponse>;