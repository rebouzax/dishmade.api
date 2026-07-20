using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Realtime;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Public.ServiceRequests;
using MediatR;

namespace dishmade.application.Features.Public.ServiceRequests.Commands.CancelServiceRequest;

public sealed class CancelServiceRequestCommandHandler
    : IRequestHandler<CancelServiceRequestCommand, ServiceRequestResponse>
{
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IKitchenRealtimeNotifier _kitchenRealtimeNotifier;

    public CancelServiceRequestCommandHandler(
        IServiceRequestRepository serviceRequestRepository,
        IUnitOfWork unitOfWork,
        IKitchenRealtimeNotifier kitchenRealtimeNotifier)
    {
        _serviceRequestRepository = serviceRequestRepository;
        _unitOfWork = unitOfWork;
        _kitchenRealtimeNotifier = kitchenRealtimeNotifier;
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

        var response = ServiceRequestMapper.ToResponse(serviceRequest);

        await _kitchenRealtimeNotifier.NotifyServiceRequestUpdatedAsync(
            serviceRequest.RestaurantId,
            response,
            cancellationToken);

        return response;
    }
}