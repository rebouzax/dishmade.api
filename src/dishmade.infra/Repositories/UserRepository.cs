using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Pagination;
using dishmade.domain.Constants;
using dishmade.domain.Entities;
using dishmade.infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace dishmade.infra.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly DishmadeDbContext _context;

    public UserRepository(DishmadeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AppUser user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public async Task<AppUser?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return await _context.Users
            .Include(user => user.Restaurant)
            .FirstOrDefaultAsync(user => user.Email == normalizedEmail, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return await _context.Users
            .AnyAsync(user => user.Email == normalizedEmail, cancellationToken);
    }

    public async Task<PagedResult<AppUser>> GetClientsPagedAsync(
        string? search,
        bool? isActive,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Users
            .AsNoTracking()
            .Include(user => user.Restaurant)
            .Where(user => user.Role == Roles.Client)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search.Trim()}%";

            query = query.Where(user =>
                EF.Functions.Like(user.Name, term) ||
                EF.Functions.Like(user.Email, term) ||
                user.Restaurant != null && EF.Functions.Like(user.Restaurant.Name, term));
        }

        if (isActive.HasValue)
        {
            query = query.Where(user => user.IsActive == isActive.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(user => user.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<AppUser>(items, totalCount);
    }
}