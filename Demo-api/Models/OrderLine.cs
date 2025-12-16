using System.Text.Json.Serialization;

namespace Demo_api.Models;

public class OrderLine
{
    public string Id { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public double Discount { get; set; }
    
    // Foreign keys
    public string OrderId { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    
    // Navigation properties
    [JsonIgnore]
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}

