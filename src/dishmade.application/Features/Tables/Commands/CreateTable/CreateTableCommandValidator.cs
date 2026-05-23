using FluentValidation;

namespace dishmade.application.Features.Tables.Commands.CreateTable;

public sealed class CreateTableCommandValidator : AbstractValidator<CreateTableCommand>
{
    public CreateTableCommandValidator()
    {
        RuleFor(command => command.Number)
            .GreaterThan(0)
            .WithMessage("O número da mesa deve ser maior que zero.");
    }
}