using FluentValidation;

namespace dishmade.application.Features.Orders.Commands.UpdateOrderItemStatus;

public sealed class UpdateOrderItemStatusCommandValidator
    : AbstractValidator<UpdateOrderItemStatusCommand>
{
    public UpdateOrderItemStatusCommandValidator()
    {
        RuleFor(command => command.OrderId)
            .NotEmpty()
            .WithMessage("O pedido é obrigatório.");

        RuleFor(command => command.OrderItemId)
            .NotEmpty()
            .WithMessage("O item do pedido é obrigatório.");

        RuleFor(command => command.Status)
            .IsInEnum()
            .WithMessage("Status do item inválido.");
    }
}