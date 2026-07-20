using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Realtime;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Exceptions;
using dishmade.application.Common.Security;
using dishmade.application.Features.Kitchen;
using dishmade.domain.Entities;
using MediatR;

namespace dishmade.application.Features.Public.Orders.Commands.OpenOrCreatePublicOrder;

public sealed class OpenOrCreatePublicOrderCommandHandler
    : IRequestHandler<OpenOrCreatePublicOrderCommand, PublicOrderSessionResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IRestaurantTableRepository _tableRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IKitchenRealtimeNotifier _kitchenRealtimeNotifier;

    public OpenOrCreatePublicOrderCommandHandler(
        IRestaurantRepository restaurantRepository,
        IRestaurantTableRepository tableRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IKitchenRealtimeNotifier kitchenRealtimeNotifier    )
    {
        _restaurantRepository = restaurantRepository;
        _tableRepository = tableRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _kitchenRealtimeNotifier = kitchenRealtimeNotifier;
    }

    public async Task<PublicOrderSessionResponse> Handle(
        OpenOrCreatePublicOrderCommand request,
        CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetBySlugAsync(
            request.RestaurantSlug,
            cancellationToken);

        if (restaurant is null || !restaurant.IsActive)
            throw new KeyNotFoundException("Restaurante não encontrado.");

        if (!restaurant.AcceptsQrCodeOrders)
            throw new InvalidOperationException("Este restaurante não aceita pedidos pelo QR Code no momento.");

        var table = await _tableRepository.GetPublicByRestaurantIdAndNumberAsync(
            restaurant.Id,
            request.TableNumber,
            cancellationToken);

        if (table is null)
            throw new KeyNotFoundException("Mesa não encontrada.");

        if (!table.IsMenuQrCodeEnabled)
            throw new InvalidOperationException("O QR Code do cardápio não está habilitado para esta mesa.");

        var openOrder = await _orderRepository.GetOpenByRestaurantIdAndTableIdAsync(
            restaurant.Id,
            table.Id,
            cancellationToken);

        if (openOrder is not null)
        {
            if (string.IsNullOrWhiteSpace(request.AccessCode))
            {
                throw new ConflictException(
                    "A mesa já possui um pedido aberto. Para recuperar o pedido, informe o código de acesso.");
            }

            if (openOrder.PublicAccessCode != request.AccessCode)
            {
                throw new ConflictException(
                    "A mesa já possui um pedido aberto, mas o código de acesso informado é inválido.");
            }

            var recoveredOrder = await _orderRepository.GetOpenDetailsByRestaurantIdAndTableIdAndAccessCodeAsync(
                restaurant.Id,
                table.Id,
                request.AccessCode,
                cancellationToken);

            if (recoveredOrder is null)
                throw new KeyNotFoundException("Pedido aberto não encontrado.");

            return new PublicOrderSessionResponse(
                WasCreated: false,
                WasRecovered: true,
                Order: PublicOrderMapper.ToResponse(recoveredOrder, restaurant));
        }

        if (table.IsOccupied)
        {
            throw new ConflictException(
                "A mesa está ocupada, mas não foi encontrado um pedido público aberto para recuperação.");
        }

        var order = new Order(table.Id, restaurant.Id);

        order.SetPublicAccessCode(PublicAccessCodeGenerator.Generate());

        table.Occupy();

        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var createdOrder = await _orderRepository.GetPublicByIdAndAccessCodeAsync(
            order.Id,
            order.PublicAccessCode!,
            cancellationToken);

        if (createdOrder is null)
            throw new KeyNotFoundException("Pedido criado não encontrado.");

        await _kitchenRealtimeNotifier.NotifyOrderCreatedAsync(
            restaurant.Id,
            KitchenOrderRealtimeMapper.ToResponse(createdOrder),
            cancellationToken);

        return new PublicOrderSessionResponse(
            WasCreated: true,
            WasRecovered: false,
            Order: PublicOrderMapper.ToResponse(createdOrder, restaurant));
    }
}