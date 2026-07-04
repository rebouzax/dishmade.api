using FluentValidation;

namespace dishmade.application.Features.Public.Orders.Commands.AddItemToPublicOrder;

public sealed class AddItemToPublicOrderCommandValidator : AbstractValidator<AddItemToPublicOrderCommand>
{
    public AddItemToPublicOrderCommandValidator()
    {
        RuleFor(command => command.OrderId)
            .NotEmpty()
            .WithMessage("O pedido é obrigatório.");

        RuleFor(command => command.AccessCode)
            .NotEmpty()
            .WithMessage("O código de acesso do pedido é obrigatório.");

        RuleFor(command => command.DishId)
            .NotEmpty()
            .WithMessage("O prato é obrigatório.");

        RuleFor(command => command.Quantity)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero.");

        RuleFor(command => command.Notes)
            .MaximumLength(500)
            .WithMessage("A observação do item deve ter no máximo 500 caracteres.");
    }
}