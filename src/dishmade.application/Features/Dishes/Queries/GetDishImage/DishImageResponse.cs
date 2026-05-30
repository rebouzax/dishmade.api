namespace dishmade.application.Features.Dishes.Queries.GetDishImage;

public sealed record DishImageResponse(
    string FileName,
    string ContentType,
    long SizeInBytes,
    byte[] Data
);