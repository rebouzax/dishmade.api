using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using dishmade.domain.Enums;
using MediatR;

namespace dishmade.application.Features.Orders.Commands.ChangeOrderStatus;

public sealed class ChangeOrderStatusCommandHandler : IRequestHandler<ChangeOrderStatusCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeOrderStatusCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ChangeOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        switch (request.Status)
        {
            case OrderStatus.InPreparation:
                order.StartPreparation();
                break;

            case OrderStatus.Ready:
                order.MarkAsReady();
                break;

            case OrderStatus.Delivered:
                order.Deliver();
                order.Table.Release();
                break;

            default:
                throw new InvalidOperationException("Status inválido para alteração.");
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}