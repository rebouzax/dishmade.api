using dishmade.domain.Enums;
using FluentValidation;

namespace dishmade.application.Features.Public.ServiceRequests.Commands.CreatePublicServiceRequest;

public sealed class CreatePublicServiceRequestCommandValidator
    : AbstractValidator<CreatePublicServiceRequestCommand>
{
    public CreatePublicServiceRequestCommandValidator()
    {
        RuleFor(command => command.RestaurantSlug)
            .NotEmpty()
            .WithMessage("O restaurante é obrigatório.");

        RuleFor(command => command.TableNumber)
            .GreaterThan(0)
            .WithMessage("O número da mesa deve ser maior que zero.");

        RuleFor(command => command.Type)
            .IsInEnum()
            .WithMessage("Tipo de solicitação inválido.");

        RuleFor(command => command.Message)
            .MaximumLength(500)
            .WithMessage("A mensagem deve ter no máximo 500 caracteres.");

        RuleFor(command => command.Message)
            .NotEmpty()
            .When(command => command.Type == ServiceRequestType.Other)
            .WithMessage("A mensagem é obrigatória para solicitações do tipo Outro.");
    }
}