using FluentValidation;

namespace dishmade.application.Features.Public.Orders.Commands.OpenOrCreatePublicOrder;

public sealed class OpenOrCreatePublicOrderCommandValidator
    : AbstractValidator<OpenOrCreatePublicOrderCommand>
{
    public OpenOrCreatePublicOrderCommandValidator()
    {
        RuleFor(command => command.RestaurantSlug)
            .NotEmpty()
            .WithMessage("O restaurante é obrigatório.");

        RuleFor(command => command.TableNumber)
            .GreaterThan(0)
            .WithMessage("O número da mesa deve ser maior que zero.");

        RuleFor(command => command.AccessCode)
            .MaximumLength(100)
            .WithMessage("O código de acesso deve ter no máximo 100 caracteres.");
    }
}