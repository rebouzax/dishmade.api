using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Public.ServiceRequests;
using MediatR;

namespace dishmade.application.Features.Public.ServiceRequests.Commands.CancelServiceRequest;

public sealed class CancelServiceRequestCommandHandler
    : IRequestHandler<CancelServiceRequestCommand, ServiceRequestResponse>
{
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelServiceRequestCommandHandler(
        IServiceRequestRepository serviceRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRequestRepository = serviceRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceRequestResponse> Handle(
        CancelServiceRequestCommand request,
        CancellationToken cancellationToken)
    {
        var serviceRequest = await _serviceRequestRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (serviceRequest is null)
            throw new KeyNotFoundException("Solicitação não encontrada.");

        serviceRequest.Cancel();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceRequestMapper.ToResponse(serviceRequest);
    }
}