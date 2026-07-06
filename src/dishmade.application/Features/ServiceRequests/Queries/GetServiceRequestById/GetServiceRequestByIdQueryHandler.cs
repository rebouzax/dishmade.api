using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.ServiceRequests.Queries.GetServiceRequestById;

public sealed class GetServiceRequestByIdQueryHandler
    : IRequestHandler<GetServiceRequestByIdQuery, ServiceRequestResponse>
{
    private readonly IServiceRequestRepository _serviceRequestRepository;

    public GetServiceRequestByIdQueryHandler(IServiceRequestRepository serviceRequestRepository)
    {
        _serviceRequestRepository = serviceRequestRepository;
    }

    public async Task<ServiceRequestResponse> Handle(
        GetServiceRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        var serviceRequest = await _serviceRequestRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (serviceRequest is null)
            throw new KeyNotFoundException("Solicitação não encontrada.");

        return ServiceRequestMapper.ToResponse(serviceRequest);
    }
}