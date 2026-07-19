using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.domain.Entities;
using MediatR;

namespace dishmade.application.Features.Public.ServiceRequests.Commands.CreatePublicServiceRequest;

public sealed class CreatePublicServiceRequestCommandHandler
    : IRequestHandler<CreatePublicServiceRequestCommand, ServiceRequestResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePublicServiceRequestCommandHandler(
        IRestaurantRepository restaurantRepository,
        IRestaurantTableRepository tableRepository,
        IServiceRequestRepository serviceRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _restaurantRepository = restaurantRepository;
        _tableRepository = tableRepository;
        _serviceRequestRepository = serviceRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceRequestResponse> Handle(
        CreatePublicServiceRequestCommand request,
        CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetBySlugAsync(
            request.RestaurantSlug,
            cancellationToken);

        if (restaurant is null || !restaurant.IsActive)
            throw new KeyNotFoundException("Restaurante não encontrado.");

        if (!restaurant.AcceptsWaiterCall)
            throw new InvalidOperationException("Este restaurante não aceita chamadas de garçom pelo QR Code no momento.");

        var table = await _tableRepository.GetPublicByRestaurantIdAndNumberAsync(
            restaurant.Id,
            request.TableNumber,
            cancellationToken);

        if (table is null)
            throw new KeyNotFoundException("Mesa não encontrada.");

        if (!table.IsMenuQrCodeEnabled)
            throw new InvalidOperationException("O QR Code do cardápio não está habilitado para esta mesa.");

        var serviceRequest = new ServiceRequest(
            restaurant.Id,
            table.Id,
            request.Type,
            request.Message);

        await _serviceRequestRepository.AddAsync(
            serviceRequest,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceRequestMapper.ToResponse(serviceRequest);
    }
}