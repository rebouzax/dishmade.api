using dishmade.application.Abstractions.Data;
using dishmade.application.Abstractions.Repositories;
using MediatR;
using dishmade.application.Features.Orders.Services;

namespace dishmade.application.Features.Public.Orders.Commands.AddItemToPublicOrder;

public sealed class AddItemToPublicOrderCommandHandler
    : IRequestHandler<AddItemToPublicOrderCommand, PublicOrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IDishRepository _dishRepository;
    private readonly IDishOptionGroupRepository _dishOptionGroupRepository;
    private readonly IDishOptionRepository _dishOptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddItemToPublicOrderCommandHandler(
        IOrderRepository orderRepository,
        IRestaurantRepository restaurantRepository,
        IDishRepository dishRepository,
        IDishOptionGroupRepository dishOptionGroupRepository,
        IDishOptionRepository dishOptionRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _restaurantRepository = restaurantRepository;
        _dishRepository = dishRepository;
        _dishOptionGroupRepository = dishOptionGroupRepository;
        _dishOptionRepository = dishOptionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PublicOrderResponse> Handle(
        AddItemToPublicOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetPublicByIdAndAccessCodeAsync(
            request.OrderId,
            request.AccessCode,
            cancellationToken);

        if (order is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        var restaurant = await _restaurantRepository.GetByIdAsync(
            order.RestaurantId,
            cancellationToken);

        if (restaurant is null || !restaurant.IsActive)
            throw new KeyNotFoundException("Restaurante não encontrado.");

        var dish = await _dishRepository.GetPublicAvailableByIdAsync(
            request.DishId,
            order.RestaurantId,
            cancellationToken);

        if (dish is null)
            throw new KeyNotFoundException("Prato não encontrado.");

        var optionIds = request.OptionIds is null
            ? new List<Guid>()
            : request.OptionIds.Distinct().ToList();

        var groups = await _dishOptionGroupRepository.GetPublicByDishIdAsync(
            dish.Id,
            order.RestaurantId,
            cancellationToken);

        var selectedOptions = await _dishOptionRepository.GetPublicAvailableByIdsForDishAsync(
            optionIds,
            dish.Id,
            order.RestaurantId,
            cancellationToken);

        if (selectedOptions.Count != optionIds.Count)
            throw new InvalidOperationException("Uma ou mais opções selecionadas são inválidas.");

        DishOptionSelectionValidator.Validate(groups, selectedOptions);

        var item = order.AddItem(
            dish.Id,
            request.Quantity,
            dish.Price,
            request.Notes);

        foreach (var option in selectedOptions)
        {
            item.AddOption(
                option.Id,
                option.Name,
                option.AdditionalPrice);
        }

        await _orderRepository.AddItemAsync(item, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedOrder = await _orderRepository.GetPublicByIdAndAccessCodeAsync(
            request.OrderId,
            request.AccessCode,
            cancellationToken);

        if (updatedOrder is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        return PublicOrderMapper.ToResponse(updatedOrder, restaurant);
    }
}