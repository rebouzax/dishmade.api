using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Orders.Receipts;
using MediatR;

namespace dishmade.application.Features.Orders.Commands.CloseOrderAccount;

public sealed class CloseOrderAccountCommandHandler
    : IRequestHandler<CloseOrderAccountCommand, OrderReceiptResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CloseOrderAccountCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
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

        order.CloseAccount(
            request.DiscountAmount,
            request.ServiceFeeAmount);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OrderReceiptMapper.ToResponse(order);
    }
}