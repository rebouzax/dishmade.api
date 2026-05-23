using FluentValidation;

namespace dishmade.application.Features.Dishes.Commands.UpdateDish;

public sealed class UpdateDishCommandValidator : AbstractValidator<UpdateDishCommand>
{
    public UpdateDishCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("O identificador do prato é obrigatório.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("O nome do prato é obrigatório.")
            .MaximumLength(150)
            .WithMessage("O nome do prato deve ter no máximo 150 caracteres.");

        RuleFor(command => command.Description)
            .MaximumLength(1000)
            .WithMessage("A descrição do prato deve ter no máximo 1000 caracteres.");

        RuleFor(command => command.Price)
            .GreaterThan(0)
            .WithMessage("O preço do prato deve ser maior que zero.");

        RuleFor(command => command.CategoryId)
            .NotEmpty()
            .WithMessage("A categoria do prato é obrigatória.");
    }
}