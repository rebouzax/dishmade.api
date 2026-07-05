using dishmade.application.Abstractions.Repositories;
using dishmade.domain.Entities;
using dishmade.infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace dishmade.infra.Repositories;

public sealed class DishOptionGroupRepository : IDishOptionGroupRepository
{
    private readonly DishmadeDbContext _context;

    public DishOptionGroupRepository(DishmadeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(DishOptionGroup group, CancellationToken cancellationToken = default)
    {
        await _context.DishOptionGroups.AddAsync(group, cancellationToken);
    }

    public async Task<DishOptionGroup?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.DishOptionGroups
            .Include(group => group.Options)
            .FirstOrDefaultAsync(group => group.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<DishOptionGroup>> GetByDishIdAsync(
        Guid dishId,
        CancellationToken cancellationToken = default)
    {
        return await _context.DishOptionGroups
            .AsNoTracking()
            .Include(group => group.Options.Where(option => !option.IsDeleted))
            .Where(group => group.DishId == dishId)
            .OrderBy(group => group.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DishOptionGroup>> GetPublicByDishIdAsync(
        Guid dishId,
        Guid restaurantId,
        CancellationToken cancellationToken = default)
    {
        return await _context.DishOptionGroups
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Include(group => group.Options.Where(option =>
                !option.IsDeleted &&
                option.IsAvailable))
            .Where(group =>
                group.DishId == dishId &&
                group.RestaurantId == restaurantId &&
                !group.IsDeleted &&
                group.IsActive)
            .OrderBy(group => group.Name)
            .ToListAsync(cancellationToken);
    }
}