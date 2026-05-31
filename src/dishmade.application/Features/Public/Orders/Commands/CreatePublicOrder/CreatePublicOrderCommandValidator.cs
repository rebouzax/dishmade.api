using FluentValidation;

namespace dishmade.application.Features.Public.Orders.Commands.CreatePublicOrder;

public sealed class CreatePublicOrderCommandValidator : AbstractValidator<CreatePublicOrderCommand>
{
    public CreatePublicOrderCommandValidator()
    {
        RuleFor(command => command.RestaurantSlug)
            .NotEmpty()
            .WithMessage("O restaurante é obrigatório.");

        RuleFor(command => command.TableNumber)
            .GreaterThan(0)
            .WithMessage("O número da mesa deve ser maior que zero.");
    }
}