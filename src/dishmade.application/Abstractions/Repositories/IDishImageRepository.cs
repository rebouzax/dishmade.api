using dishmade.domain.Entities;

namespace dishmade.application.Abstractions.Repositories;

public interface IDishImageRepository
{
    Task AddAsync(DishImage image, CancellationToken cancellationToken = default);

    Task<DishImage?> GetByDishIdAsync(Guid dishId, CancellationToken cancellationToken = default);

    void Remove(DishImage image);

    Task<DishImage?> GetPublicByDishIdAsync(
    Guid dishId,
    CancellationToken cancellationToken = default);
}