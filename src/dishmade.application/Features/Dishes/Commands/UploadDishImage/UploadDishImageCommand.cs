using MediatR;

namespace dishmade.application.Features.Dishes.Commands.UploadDishImage;

public sealed record UploadDishImageCommand(
    Guid DishId,
    string FileName,
    string ContentType,
    long SizeInBytes,
    byte[] Data
) : IRequest;