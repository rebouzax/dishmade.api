using dishmade.application.Abstractions.Repositories;
using dishmade.application.Features.Orders.Receipts;
using MediatR;

namespace dishmade.application.Features.Orders.Queries.GetOrderReceipt;

public sealed class GetOrderReceiptQueryHandler
    : IRequestHandler<GetOrderReceiptQuery, OrderReceiptResponse>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderReceiptQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderReceiptResponse> Handle(
        GetOrderReceiptQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        return OrderReceiptMapper.ToResponse(order);
    }
}