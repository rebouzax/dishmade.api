using dishmade.domain.Enums;
using FluentValidation;

namespace dishmade.application.Features.Orders.Commands.ChangeOrderStatus;

public sealed class ChangeOrderStatusCommandValidator : AbstractValidator<ChangeOrderStatusCommand>
{
    public ChangeOrderStatusCommandValidator()
    {
        RuleFor(command => command.OrderId)
            .NotEmpty()
            .WithMessage("O pedido é obrigatório.");

        RuleFor(command => command.Status)
            .Must(status => status is OrderStatus.InPreparation or OrderStatus.Ready or OrderStatus.Delivered)
            .WithMessage("Status inválido para alteração direta.");
    }
}