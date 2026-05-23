using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Pagination;
using dishmade.domain.Entities;
using dishmade.infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace dishmade.infra.Repositories;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly DishmadeDbContext _context;

    public CategoryRepository(DishmadeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<Category>> GetPagedAsync(
    string? search,
    bool? isActive,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default)
    {
        var query = _context.Categories
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search.Trim()}%";

            query = query.Where(category =>
                EF.Functions.Like(category.Name, term) ||
                category.Description != null && EF.Functions.Like(category.Description, term));
        }

        if (isActive.HasValue)
        {
            query = query.Where(category => category.IsActive == isActive.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(category => category.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Category>(items, totalCount);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AnyAsync(category => category.Name == name, cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AnyAsync(category => category.Id == id, cancellationToken);
    }
}