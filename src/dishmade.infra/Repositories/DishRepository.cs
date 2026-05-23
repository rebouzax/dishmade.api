using dishmade.application.Abstractions.Repositories;
using dishmade.domain.Entities;
using dishmade.infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace dishmade.infra.Repositories;

public sealed class DishRepository : IDishRepository
{
    private readonly DishmadeDbContext _context;

    public DishRepository(DishmadeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Dish dish, CancellationToken cancellationToken = default)
    {
        await _context.Dishes.AddAsync(dish, cancellationToken);
    }

    public async Task<Dish?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Dishes
            .Include(dish => dish.Category)
            .FirstOrDefaultAsync(dish => dish.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Dish>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Dishes
            .AsNoTracking()
            .Include(dish => dish.Category)
            .OrderBy(dish => dish.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(
        string name,
        Guid? ignoredDishId = null,
        CancellationToken cancellationToken = default)
    {
        return await _context.Dishes
            .AnyAsync(dish =>
                    dish.Name == name &&
                    (!ignoredDishId.HasValue || dish.Id != ignoredDishId.Value),
                cancellationToken);
    }
}