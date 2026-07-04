using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;

namespace dishmade.application.Features.Orders.Commands.AddItemToOrder;

public sealed class AddItemToOrderCommandHandler : IRequestHandler<AddItemToOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDishRepository _dishRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddItemToOrderCommandHandler(
        IOrderRepository orderRepository,
        IDishRepository dishRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _dishRepository = dishRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        AddItemToOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        var dish = await _dishRepository.GetByIdAsync(request.DishId, cancellationToken);

        if (dish is null)
            throw new KeyNotFoundException("Prato não encontrado.");

        if (!dish.IsAvailable)
            throw new InvalidOperationException("Não é possível adicionar um prato indisponível ao pedido.");

        var item = order.AddItem(
            dish.Id,
            request.Quantity,
            dish.Price,
            request.Notes);

        await _orderRepository.AddItemAsync(item, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}