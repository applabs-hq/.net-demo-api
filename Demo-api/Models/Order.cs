namespace Demo_api.Models;

public class Order
{
    public string Id { get; set; } = string.Empty;
    public DateTime CreatedAtUTC { get; set; }
    public string CurrencyCode { get; set; } = "NZD";
    
    // Navigation property
    public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();
}

