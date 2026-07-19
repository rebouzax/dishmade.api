using FluentValidation;

namespace dishmade.application.Features.Orders.Commands.CloseOrderAccount;

public sealed class CloseOrderAccountCommandValidator : AbstractValidator<CloseOrderAccountCommand>
{
    public CloseOrderAccountCommandValidator()
    {
        RuleFor(command => command.OrderId)
            .NotEmpty()
            .WithMessage("O pedido é obrigatório.");

        RuleFor(command => command.DiscountAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("O desconto não pode ser negativo.");

        RuleFor(command => command.ServiceFeeAmount)
            .GreaterThanOrEqualTo(0)
            .When(command => command.ServiceFeeAmount.HasValue)
            .WithMessage("A taxa de serviço não pode ser negativa.");
    }
}