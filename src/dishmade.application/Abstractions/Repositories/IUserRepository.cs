using dishmade.application.Common.Pagination;
using dishmade.domain.Entities;

namespace dishmade.application.Abstractions.Repositories;

public interface IUserRepository
{
    Task AddAsync(AppUser user, CancellationToken cancellationToken = default);

    Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<PagedResult<AppUser>> GetClientsPagedAsync(
        string? search,
        bool? isActive,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}