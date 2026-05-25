using FluentValidation;

namespace dishmade.application.Features.Admin.Clients.Commands.CreateClient;

public sealed class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(command => command.RestaurantName)
            .NotEmpty()
            .WithMessage("O nome do restaurante é obrigatório.")
            .MaximumLength(150)
            .WithMessage("O nome do restaurante deve ter no máximo 150 caracteres.");

        RuleFor(command => command.UserName)
            .NotEmpty()
            .WithMessage("O nome do usuário é obrigatório.")
            .MaximumLength(150)
            .WithMessage("O nome do usuário deve ter no máximo 150 caracteres.");

        RuleFor(command => command.Email)
            .NotEmpty()
            .WithMessage("O e-mail é obrigatório.")
            .EmailAddress()
            .WithMessage("E-mail inválido.");

        RuleFor(command => command.Password)
            .NotEmpty()
            .WithMessage("A senha é obrigatória.")
            .MinimumLength(8)
            .WithMessage("A senha deve ter no mínimo 8 caracteres.");
    }
}