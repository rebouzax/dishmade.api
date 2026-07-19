using FluentValidation;

namespace dishmade.application.Features.RestaurantSettings.Commands.UpdateRestaurantSettings;

public sealed class UpdateRestaurantSettingsCommandValidator
    : AbstractValidator<UpdateRestaurantSettingsCommand>
{
    public UpdateRestaurantSettingsCommandValidator()
    {
        RuleFor(command => command.DefaultServiceFeePercentage)
            .GreaterThanOrEqualTo(0)
            .WithMessage("A taxa de serviço padrão não pode ser negativa.")
            .LessThanOrEqualTo(100)
            .WithMessage("A taxa de serviço padrão não pode ser maior que 100%.");
    }
}