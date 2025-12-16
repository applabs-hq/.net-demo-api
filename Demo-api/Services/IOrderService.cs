using Demo_api.Models;

namespace Demo_api.Services;

public interface IOrderService
{
    OrderDto ToDto(Order order);
    IEnumerable<OrderDto> ToDto(IEnumerable<Order> orders);
}

