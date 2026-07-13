using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Orders.Receipts;
using dishmade.domain.Enums;
using MediatR;

namespace dishmade.application.Features.Orders.Commands.RegisterOrderPayment;

public sealed class RegisterOrderPaymentCommandHandler
    : IRequestHandler<RegisterOrderPaymentCommand, OrderReceiptResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterOrderPaymentCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderReceiptResponse> Handle(
        RegisterOrderPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        var payment = order.RegisterPayment(
            request.Method,
            request.Amount,
            request.Notes);

        await _orderRepository.AddPaymentAsync(payment, cancellationToken);

        order.MarkAsPaidIfFullyPaid();

        if (order.PaymentStatus == PaymentStatus.Paid)
        {
            order.Deliver();

            if (order.Table.IsOccupied)
                order.Table.Release();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OrderReceiptMapper.ToResponse(order);
    }
}