using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Public.ServiceRequests;
using MediatR;

namespace dishmade.application.Features.Public.ServiceRequests.Commands.StartServiceRequest;

public sealed class StartServiceRequestCommandHandler
    : IRequestHandler<StartServiceRequestCommand, ServiceRequestResponse>
{
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartServiceRequestCommandHandler(
        IServiceRequestRepository serviceRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRequestRepository = serviceRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceRequestResponse> Handle(
        StartServiceRequestCommand request,
        CancellationToken cancellationToken)
    {
        var serviceRequest = await _serviceRequestRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (serviceRequest is null)
            throw new KeyNotFoundException("Solicitação não encontrada.");

        serviceRequest.Start();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceRequestMapper.ToResponse(serviceRequest);
    }
}