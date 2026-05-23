using dishmade.application.Abstractions.Data;
using dishmade.infra.Data.Context;

namespace dishmade.infra.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DishmadeDbContext _context;

    public UnitOfWork(DishmadeDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}