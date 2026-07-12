using FluentValidation;

namespace dishmade.application.Features.Public.Orders.Queries.GetCurrentPublicOrderByTable;

public sealed class GetCurrentPublicOrderByTableQueryValidator
    : AbstractValidator<GetCurrentPublicOrderByTableQuery>
{
    public GetCurrentPublicOrderByTableQueryValidator()
    {
        RuleFor(query => query.RestaurantSlug)
            .NotEmpty()
            .WithMessage("O restaurante é obrigatório.");

        RuleFor(query => query.TableNumber)
            .GreaterThan(0)
            .WithMessage("O número da mesa deve ser maior que zero.");

        RuleFor(query => query.AccessCode)
            .NotEmpty()
            .WithMessage("O código de acesso é obrigatório.");
    }
}