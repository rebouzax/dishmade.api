using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Pagination;
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

    public async Task<PagedResult<Dish>> GetPagedAsync(
    string? search,
    Guid? categoryId,
    bool? isAvailable,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default)
    {
        var query = _context.Dishes
            .AsNoTracking()
            .Include(dish => dish.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search.Trim()}%";

            query = query.Where(dish =>
                EF.Functions.Like(dish.Name, term) ||
                dish.Description != null && EF.Functions.Like(dish.Description, term));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(dish => dish.CategoryId == categoryId.Value);
        }

        if (isAvailable.HasValue)
        {
            query = query.Where(dish => dish.IsAvailable == isAvailable.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(dish => dish.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Dish>(items, totalCount);
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

    public async Task<IReadOnlyList<Dish>> GetPublicByRestaurantIdAsync(
    Guid restaurantId,
    Guid? categoryId = null,
    CancellationToken cancellationToken = default)
    {
        var query = _context.Dishes
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Include(dish => dish.Category)
            .Where(dish =>
                dish.RestaurantId == restaurantId &&
                !dish.IsDeleted &&
                dish.IsAvailable &&
                dish.Category.IsActive);

        if (categoryId.HasValue)
        {
            query = query.Where(dish => dish.CategoryId == categoryId.Value);
        }

        return await query
            .OrderBy(dish => dish.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dish?> GetPublicAvailableByIdAsync(
        Guid dishId,
        Guid restaurantId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Dishes
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Include(dish => dish.Category)
            .FirstOrDefaultAsync(
                dish =>
                    dish.Id == dishId &&
                    dish.RestaurantId == restaurantId &&
                    !dish.IsDeleted &&
                    dish.IsAvailable &&
                    dish.Category.IsActive,
                cancellationToken);
    }
}