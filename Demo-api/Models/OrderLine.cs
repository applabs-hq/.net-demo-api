namespace Demo_api.Models;

public class OrderLine
{
    public string Id { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Product Product { get; set; } = null!;
    public double Discount { get; set; }
}

