using FluentValidation;

namespace dishmade.application.Features.Dishes.OptionGroups.Commands.UpdateDishOptionGroup;

public sealed class UpdateDishOptionGroupCommandValidator : AbstractValidator<UpdateDishOptionGroupCommand>
{
    public UpdateDishOptionGroupCommandValidator()
    {
        RuleFor(command => command.DishId)
            .NotEmpty()
            .WithMessage("O prato é obrigatório.");

        RuleFor(command => command.OptionGroupId)
            .NotEmpty()
            .WithMessage("O grupo de opções é obrigatório.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("O nome do grupo é obrigatório.")
            .MaximumLength(120)
            .WithMessage("O nome do grupo deve ter no máximo 120 caracteres.");

        RuleFor(command => command.MinSelection)
            .GreaterThanOrEqualTo(0)
            .WithMessage("A seleção mínima não pode ser negativa.");

        RuleFor(command => command.MaxSelection)
            .GreaterThan(0)
            .WithMessage("A seleção máxima deve ser maior que zero.");

        RuleFor(command => command)
            .Must(command => command.MinSelection <= command.MaxSelection)
            .WithMessage("A seleção mínima não pode ser maior que a seleção máxima.");

        RuleFor(command => command)
            .Must(command => !command.IsRequired || command.MinSelection > 0)
            .WithMessage("Grupos obrigatórios devem ter seleção mínima maior que zero.");
    }
}