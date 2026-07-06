using dishmade.application.Abstractions.Repositories;
using dishmade.application.Common.Pagination;
using dishmade.domain.Entities;
using dishmade.domain.Enums;
using dishmade.infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace dishmade.infra.Repositories;

public sealed class ServiceRequestRepository : IServiceRequestRepository
{
    private readonly DishmadeDbContext _context;

    public ServiceRequestRepository(DishmadeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        ServiceRequest request,
        CancellationToken cancellationToken = default)
    {
        await _context.ServiceRequests.AddAsync(request, cancellationToken);
    }

    public async Task<ServiceRequest?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.ServiceRequests
            .Include(request => request.Table)
            .FirstOrDefaultAsync(request => request.Id == id, cancellationToken);
    }

    public async Task<PagedResult<ServiceRequest>> GetPagedAsync(
        ServiceRequestStatus? status,
        ServiceRequestType? type,
        Guid? tableId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.ServiceRequests
            .AsNoTracking()
            .Include(request => request.Table)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(request => request.Status == status.Value);
        }

        if (type.HasValue)
        {
            query = query.Where(request => request.Type == type.Value);
        }

        if (tableId.HasValue)
        {
            query = query.Where(request => request.TableId == tableId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(request => request.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<ServiceRequest>(
            items,
            totalCount);
    }
}