using FluentValidation;

namespace dishmade.application.Features.Tables.Commands.UpdateTable;

public sealed class UpdateTableCommandValidator : AbstractValidator<UpdateTableCommand>
{
    public UpdateTableCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("O identificador da mesa é obrigatório.");

        RuleFor(command => command.Number)
            .GreaterThan(0)
            .WithMessage("O número da mesa deve ser maior que zero.");
    }
}