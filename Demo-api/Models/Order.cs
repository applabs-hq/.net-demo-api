namespace Demo_api.Models;

public class Order
{
    public string Id { get; set; } = string.Empty;
    public DateTime CreatedAtUTC { get; set; }
    public List<OrderLine> Lines { get; set; } = new();
    public string CurrencyCode { get; set; } = "NZD";
}

