using dishmade.application.Abstractions.Repositories;
using dishmade.domain.Entities;
using dishmade.infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace dishmade.infra.Repositories;

public sealed class DishImageRepository : IDishImageRepository
{
    private readonly DishmadeDbContext _context;

    public DishImageRepository(DishmadeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(DishImage image, CancellationToken cancellationToken = default)
    {
        await _context.DishImages.AddAsync(image, cancellationToken);
    }

    public async Task<DishImage?> GetByDishIdAsync(
        Guid dishId,
        CancellationToken cancellationToken = default)
    {
        return await _context.DishImages
            .FirstOrDefaultAsync(image => image.DishId == dishId, cancellationToken);
    }

    public void Remove(DishImage image)
    {
        _context.DishImages.Remove(image);
    }
}