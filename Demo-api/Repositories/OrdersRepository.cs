using Microsoft.EntityFrameworkCore;
using Demo_api.Data;
using Demo_api.Models;

namespace Demo_api.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly OrdersDbContext _context;

    public OrdersRepository(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Lines)
                .ThenInclude(l => l.Product)
            .OrderByDescending(o => o.CreatedAtUTC)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(string id)
    {
        return await _context.Orders
            .Include(o => o.Lines)
                .ThenInclude(l => l.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}

