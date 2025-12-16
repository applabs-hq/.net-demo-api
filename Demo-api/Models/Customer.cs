using System.Text.Json.Serialization;

namespace Demo_api.Models;

public class Customer
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    
    // Navigation property
    [JsonIgnore]
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

