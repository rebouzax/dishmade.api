using FluentValidation;

namespace dishmade.application.Features.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("O nome da categoria é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome da categoria deve ter no máximo 100 caracteres.");

        RuleFor(command => command.Description)
            .MaximumLength(500)
            .WithMessage("A descrição da categoria deve ter no máximo 500 caracteres.");

        RuleFor(command => command.RestaurantId)
            .NotEmpty()
            .WithMessage("O restaurante é obrigatório.");
    }
}