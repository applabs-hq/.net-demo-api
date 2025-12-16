namespace Demo_api.Models;

public class OrderDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime CreatedAtUTC { get; set; }
    public string CurrencyCode { get; set; } = "NZD";
    
    public ICollection<OrderLineDto> Lines { get; set; } = new List<OrderLineDto>();
}

public class OrderLineDto
{
    public string Id { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public double Discount { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public ProductDto Product { get; set; } = null!;
}

public class ProductDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Tax { get; set; }
    public double Price { get; set; }
}

