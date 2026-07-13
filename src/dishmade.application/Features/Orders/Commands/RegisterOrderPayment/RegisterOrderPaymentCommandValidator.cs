using FluentValidation;

namespace dishmade.application.Features.Orders.Commands.RegisterOrderPayment;

public sealed class RegisterOrderPaymentCommandValidator : AbstractValidator<RegisterOrderPaymentCommand>
{
    public RegisterOrderPaymentCommandValidator()
    {
        RuleFor(command => command.OrderId)
            .NotEmpty()
            .WithMessage("O pedido é obrigatório.");

        RuleFor(command => command.Method)
            .IsInEnum()
            .WithMessage("Método de pagamento inválido.");

        RuleFor(command => command.Amount)
            .GreaterThan(0)
            .WithMessage("O valor do pagamento deve ser maior que zero.");

        RuleFor(command => command.Notes)
            .MaximumLength(500)
            .WithMessage("A observação do pagamento deve ter no máximo 500 caracteres.");
    }
}