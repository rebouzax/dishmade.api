using FluentValidation;

namespace dishmade.application.Features.Auth.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .WithMessage("O e-mail é obrigatório.")
            .EmailAddress()
            .WithMessage("E-mail inválido.");

        RuleFor(command => command.Password)
            .NotEmpty()
            .WithMessage("A senha é obrigatória.");
    }
}