using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Pagination;
using dishmade.domain.Entities;
using dishmade.infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace dishmade.infra.Repositories;

public sealed class RestaurantTableRepository : IRestaurantTableRepository
{
    private readonly DishmadeDbContext _context;

    public RestaurantTableRepository(DishmadeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RestaurantTable table, CancellationToken cancellationToken = default)
    {
        await _context.RestaurantTables.AddAsync(table, cancellationToken);
    }

    public async Task<RestaurantTable?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.RestaurantTables
            .FirstOrDefaultAsync(table => table.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<RestaurantTable>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.RestaurantTables
            .AsNoTracking()
            .OrderBy(table => table.Number)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<RestaurantTable>> GetPagedAsync(
    int? number,
    bool? isOccupied,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default)
    {
        var query = _context.RestaurantTables
            .AsNoTracking()
            .AsQueryable();

        if (number.HasValue)
        {
            query = query.Where(table => table.Number == number.Value);
        }

        if (isOccupied.HasValue)
        {
            query = query.Where(table => table.IsOccupied == isOccupied.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(table => table.Number)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<RestaurantTable>(items, totalCount);
    }


    public async Task<bool> ExistsByNumberAsync(
        int number,
        Guid? ignoredTableId = null,
        CancellationToken cancellationToken = default)
    {
        return await _context.RestaurantTables
            .AnyAsync(table =>
                    table.Number == number &&
                    (!ignoredTableId.HasValue || table.Id != ignoredTableId.Value),
                cancellationToken);
    }
}