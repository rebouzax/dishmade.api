using FluentValidation;

namespace dishmade.application.Features.Dishes.Commands.UploadDishImage;

public sealed class UploadDishImageCommandValidator : AbstractValidator<UploadDishImageCommand>
{
    private const long MaxImageSizeInBytes = 5 * 1024 * 1024;

    private static readonly string[] AllowedContentTypes =
    [
        "image/jpeg",
        "image/png",
        "image/webp"
    ];

    public UploadDishImageCommandValidator()
    {
        RuleFor(command => command.DishId)
            .NotEmpty()
            .WithMessage("O prato é obrigatório.");

        RuleFor(command => command.FileName)
            .NotEmpty()
            .WithMessage("O nome do arquivo é obrigatório.")
            .MaximumLength(255)
            .WithMessage("O nome do arquivo deve ter no máximo 255 caracteres.");

        RuleFor(command => command.ContentType)
            .NotEmpty()
            .WithMessage("O tipo do arquivo é obrigatório.")
            .Must(contentType => AllowedContentTypes.Contains(contentType))
            .WithMessage("Formato de imagem inválido. Use JPEG, PNG ou WEBP.");

        RuleFor(command => command.SizeInBytes)
            .GreaterThan(0)
            .WithMessage("A imagem não pode estar vazia.")
            .LessThanOrEqualTo(MaxImageSizeInBytes)
            .WithMessage("A imagem deve ter no máximo 5MB.");

        RuleFor(command => command.Data)
            .NotEmpty()
            .WithMessage("Os dados da imagem são obrigatórios.");
    }
}