using dishmade.domain.Entities;

namespace dishmade.application.Abstractions.Repositories;

public interface IRestaurantRepository
{
    Task AddAsync(Restaurant restaurant, CancellationToken cancellationToken = default);

    Task<Restaurant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}