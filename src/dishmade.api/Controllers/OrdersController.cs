using dishmade.application.Features.Orders.Commands.AddItemToOrder;
using dishmade.application.Features.Orders.Commands.CancelOrder;
using dishmade.application.Features.Orders.Commands.ChangeOrderStatus;
using dishmade.application.Features.Orders.Commands.CloseOrderAccount;
using dishmade.application.Features.Orders.Commands.CreateOrder;
using dishmade.application.Features.Orders.Commands.RegisterOrderPayment;
using dishmade.application.Features.Orders.Commands.UpdateOrderItemStatus;
using dishmade.application.Features.Orders.Queries.GetOrderById;
using dishmade.application.Features.Orders.Queries.GetOrderReceipt;
using dishmade.application.Features.Orders.Queries.GetOrders;
using dishmade.domain.Constants;
using dishmade.domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dishmade.api.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize(Roles = Roles.Client)]
public sealed class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand(request.TableId);

        var orderId = await _sender.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = orderId },
            new { id = orderId });
    }

    [HttpPost("{id:guid}/items")]
    public async Task<IActionResult> AddItem(
        Guid id,
        [FromBody] AddItemToOrderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddItemToOrderCommand(
            id,
            request.DishId,
            request.Quantity,
            request.Notes);

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] OrderStatus? status,
    [FromQuery] Guid? tableId,
    [FromQuery] DateTime? startDate,
    [FromQuery] DateTime? endDate,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        var orders = await _sender.Send(
            new GetOrdersQuery(status, tableId, startDate, endDate, pageNumber, pageSize),
            cancellationToken);

        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var order = await _sender.Send(new GetOrderByIdQuery(id), cancellationToken);

        return Ok(order);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        [FromBody] ChangeOrderStatusRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangeOrderStatusCommand(id, request.Status);

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new CancelOrderCommand(id), cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:guid}/close")]
    public async Task<IActionResult> CloseAccount(
    Guid id,
    [FromBody] CloseOrderAccountRequest request,
    CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CloseOrderAccountCommand(
                id,
                request.DiscountAmount,
                request.ServiceFeeAmount,
                request.UseDefaultServiceFee),
            cancellationToken);

        return Ok(response);
    }

    [HttpPost("{id:guid}/payments")]
    public async Task<IActionResult> RegisterPayment(
        Guid id,
        [FromBody] RegisterOrderPaymentRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new RegisterOrderPaymentCommand(
                id,
                request.Method,
                request.Amount,
                request.Notes),
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:guid}/receipt")]
    public async Task<IActionResult> GetReceipt(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new GetOrderReceiptQuery(id),
            cancellationToken);

        return Ok(response);
    }

    [HttpPatch("{orderId:guid}/items/{itemId:guid}/status")]
    public async Task<IActionResult> UpdateItemStatus(
    Guid orderId,
    Guid itemId,
    [FromBody] UpdateOrderItemStatusRequest request,
    CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new UpdateOrderItemStatusCommand(
                orderId,
                itemId,
                request.Status),
            cancellationToken);

        return Ok(response);
    }

}

public sealed record CreateOrderRequest(Guid TableId);

public sealed record AddItemToOrderRequest(
    Guid DishId,
    int Quantity,
    string? Notes
);
public sealed record UpdateOrderItemStatusRequest(
    OrderItemStatus Status
);
public sealed record ChangeOrderStatusRequest(OrderStatus Status);

public sealed record CloseOrderAccountRequest(
    decimal DiscountAmount,
    decimal? ServiceFeeAmount,
    bool UseDefaultServiceFee
);

public sealed record RegisterOrderPaymentRequest(
    PaymentMethod Method,
    decimal Amount,
    string? Notes
);