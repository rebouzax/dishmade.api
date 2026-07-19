using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Orders.Receipts;
using MediatR;

namespace dishmade.application.Features.Orders.Commands.CloseOrderAccount;

public sealed class CloseOrderAccountCommandHandler
    : IRequestHandler<CloseOrderAccountCommand, OrderReceiptResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CloseOrderAccountCommandHandler(
        IOrderRepository orderRepository,
        IRestaurantRepository restaurantRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _restaurantRepository = restaurantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderReceiptResponse> Handle(
        CloseOrderAccountCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        var restaurant = await _restaurantRepository.GetByIdAsync(
            order.RestaurantId,
            cancellationToken);

        if (restaurant is null || !restaurant.IsActive)
            throw new KeyNotFoundException("Restaurante não encontrado.");

        var subtotal = order.GetSubtotal();

        var serviceFeeAmount = request.UseDefaultServiceFee
            ? Math.Round(subtotal * restaurant.DefaultServiceFeePercentage / 100, 2)
            : request.ServiceFeeAmount ?? 0;

        order.CloseAccount(
            request.DiscountAmount,
            serviceFeeAmount);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OrderReceiptMapper.ToResponse(order);
    }
}