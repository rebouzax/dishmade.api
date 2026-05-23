using FluentValidation;

namespace dishmade.application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(command => command.TableId)
            .NotEmpty()
            .WithMessage("A mesa é obrigatória para criar um pedido.");
    }
}