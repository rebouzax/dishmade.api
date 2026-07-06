using FluentValidation;

namespace dishmade.application.Features.Dishes.Options.Commands.UpdateDishOption;

public sealed class UpdateDishOptionCommandValidator : AbstractValidator<UpdateDishOptionCommand>
{
    public UpdateDishOptionCommandValidator()
    {
        RuleFor(command => command.DishId)
            .NotEmpty()
            .WithMessage("O prato é obrigatório.");

        RuleFor(command => command.OptionGroupId)
            .NotEmpty()
            .WithMessage("O grupo de opções é obrigatório.");

        RuleFor(command => command.OptionId)
            .NotEmpty()
            .WithMessage("A opção é obrigatória.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("O nome da opção é obrigatório.")
            .MaximumLength(120)
            .WithMessage("O nome da opção deve ter no máximo 120 caracteres.");

        RuleFor(command => command.AdditionalPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("O preço adicional não pode ser negativo.");
    }
}