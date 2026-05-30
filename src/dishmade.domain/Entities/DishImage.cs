using dishmade.domain.Common;

namespace dishmade.domain.Entities;

public sealed class DishImage : RestaurantScopedEntity
{
    public Guid DishId { get; private set; }
    public Dish Dish { get; private set; } = null!;

    public string FileName { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public long SizeInBytes { get; private set; }
    public byte[] Data { get; private set; } = [];

    private DishImage()
    {
    }

    public DishImage(
        Guid dishId,
        Guid restaurantId,
        string fileName,
        string contentType,
        long sizeInBytes,
        byte[] data)
    {
        SetRestaurantId(restaurantId);

        DishId = dishId;
        FileName = fileName.Trim();
        ContentType = contentType.Trim();
        SizeInBytes = sizeInBytes;
        Data = data;
    }

    public void Update(
        string fileName,
        string contentType,
        long sizeInBytes,
        byte[] data)
    {
        FileName = fileName.Trim();
        ContentType = contentType.Trim();
        SizeInBytes = sizeInBytes;
        Data = data;

        SetUpdatedAt();
    }
}