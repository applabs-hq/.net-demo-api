using Demo_api.Models;

namespace Demo_api.Services;

public class OrderService : IOrderService
{
    public OrderDto ToDto(Order order)
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

    public IEnumerable<OrderDto> ToDto(IEnumerable<Order> orders)
    {
        if (orders == null)
            throw new ArgumentNullException(nameof(orders));

        return orders.Select(ToDto);
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

