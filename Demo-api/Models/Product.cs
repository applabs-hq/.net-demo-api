using System.Text.Json.Serialization;

namespace Demo_api.Models;

public class Product
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Tax { get; set; }
    public double Price { get; set; }
    
    // Navigation property
    [JsonIgnore]
    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}

