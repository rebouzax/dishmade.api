using dishmade.application.Abstractions.Repositories;
using dishmade.domain.Entities;
using dishmade.infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace dishmade.infra.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly DishmadeDbContext _context;

    public OrderRepository(DishmadeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(order => order.Table)
            .Include(order => order.Items)
                .ThenInclude(item => item.Dish)
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .AsNoTracking()
            .Include(order => order.Table)
            .Include(order => order.Items)
                .ThenInclude(item => item.Dish)
            .OrderByDescending(order => order.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddItemAsync(OrderItem item, CancellationToken cancellationToken = default)
    {
        await _context.OrderItems.AddAsync(item, cancellationToken);
    }
}