using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Realtime;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Public.ServiceRequests;
using MediatR;

namespace dishmade.application.Features.Public.ServiceRequests.Commands.StartServiceRequest;

public sealed class StartServiceRequestCommandHandler
    : IRequestHandler<StartServiceRequestCommand, ServiceRequestResponse>
{
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IKitchenRealtimeNotifier _kitchenRealtimeNotifier;

    public StartServiceRequestCommandHandler(
        IServiceRequestRepository serviceRequestRepository,
        IUnitOfWork unitOfWork,
        IKitchenRealtimeNotifier kitchenRealtimeNotifier)
    {
        _serviceRequestRepository = serviceRequestRepository;
        _unitOfWork = unitOfWork;
        _kitchenRealtimeNotifier = kitchenRealtimeNotifier;
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

        var response = ServiceRequestMapper.ToResponse(serviceRequest);

        await _kitchenRealtimeNotifier.NotifyServiceRequestUpdatedAsync(
            serviceRequest.RestaurantId,
            response,
            cancellationToken);

        return response;

    }
}