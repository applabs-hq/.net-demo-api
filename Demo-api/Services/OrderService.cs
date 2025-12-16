using Demo_api.Models;
using Demo_api.Repositories;

namespace Demo_api.Services;

public class OrderService : IOrderService
{
    private readonly IOrdersRepository _ordersRepository;

    public OrderService(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _ordersRepository.GetAllOrdersAsync();
        return orders.Select(ToDto);
    }

    public async Task<OrderDto?> GetOrderByIdAsync(string id)
    {
        var order = await _ordersRepository.GetOrderByIdAsync(id);
        return order != null ? ToDto(order) : null;
    }

    // Private mapping methods - these could also be moved to a separate Mapper class
    private OrderDto ToDto(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        return new OrderDto
        {
            Id = order.Id,
            CreatedAtUTC = order.CreatedAtUTC,
            CurrencyCode = order.CurrencyCode,
            Lines = order.Lines.Select(ToDto).ToList()
        };
    }

    private OrderLineDto ToDto(OrderLine orderLine)
    {
        if (orderLine == null)
            throw new ArgumentNullException(nameof(orderLine));

        return new OrderLineDto
        {
            Id = orderLine.Id,
            Quantity = orderLine.Quantity,
            Discount = orderLine.Discount,
            ProductId = orderLine.ProductId,
            Product = ToDto(orderLine.Product)
        };
    }

    private ProductDto ToDto(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Tax = product.Tax,
            Price = product.Price
        };
    }
}

