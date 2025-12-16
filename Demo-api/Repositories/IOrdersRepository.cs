using Demo_api.Models;

namespace Demo_api.Repositories;

public interface IOrdersRepository
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(string id);
}

