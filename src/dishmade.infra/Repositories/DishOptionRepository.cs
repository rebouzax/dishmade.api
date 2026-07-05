using dishmade.application.Abstractions.Repositories;
using dishmade.domain.Entities;
using dishmade.infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace dishmade.infra.Repositories;

public sealed class DishOptionRepository : IDishOptionRepository
{
    private readonly DishmadeDbContext _context;

    public DishOptionRepository(DishmadeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(DishOption option, CancellationToken cancellationToken = default)
    {
        await _context.DishOptions.AddAsync(option, cancellationToken);
    }

    public async Task<DishOption?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.DishOptions
            .Include(option => option.OptionGroup)
            .FirstOrDefaultAsync(option => option.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<DishOption>> GetAvailableByIdsForDishAsync(
        IReadOnlyCollection<Guid> optionIds,
        Guid dishId,
        CancellationToken cancellationToken = default)
    {
        return await _context.DishOptions
            .Include(option => option.OptionGroup)
            .Where(option =>
                optionIds.Contains(option.Id) &&
                option.IsAvailable &&
                option.OptionGroup.DishId == dishId &&
                option.OptionGroup.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DishOption>> GetPublicAvailableByIdsForDishAsync(
        IReadOnlyCollection<Guid> optionIds,
        Guid dishId,
        Guid restaurantId,
        CancellationToken cancellationToken = default)
    {
        return await _context.DishOptions
            .IgnoreQueryFilters()
            .Include(option => option.OptionGroup)
            .Where(option =>
                optionIds.Contains(option.Id) &&
                option.RestaurantId == restaurantId &&
                !option.IsDeleted &&
                option.IsAvailable &&
                option.OptionGroup.DishId == dishId &&
                option.OptionGroup.RestaurantId == restaurantId &&
                !option.OptionGroup.IsDeleted &&
                option.OptionGroup.IsActive)
            .ToListAsync(cancellationToken);
    }
}