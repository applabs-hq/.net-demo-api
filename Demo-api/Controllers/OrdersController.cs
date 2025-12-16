using Microsoft.AspNetCore.Mvc;
using Demo_api.Models;

namespace Demo_api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private static readonly List<Order> _mockOrders = GenerateMockOrders();

    public OrdersController(ILogger<OrdersController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetOrders")]
    public IEnumerable<Order> Get()
    {
        return _mockOrders;
    }

    private static List<Order> GenerateMockOrders()
    {
        // Use a seeded random for deterministic results
        var random = new Random(42);
        var orders = new List<Order>();
        // Use a fixed base date for deterministic timestamps
        var baseDate = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);

        // Create a pool of products with some duplicates
        var products = new List<Product>
        {
            new Product { Id = "PROD-001", Name = "Classic Burger", Tax = 0.15, Price = 12.99 },
            new Product { Id = "PROD-002", Name = "Cheese Burger", Tax = 0.15, Price = 14.99 },
            new Product { Id = "PROD-003", Name = "Bacon Burger", Tax = 0.15, Price = 16.99 },
            new Product { Id = "PROD-004", Name = "Double Burger", Tax = 0.15, Price = 18.99 },
            new Product { Id = "PROD-005", Name = "Chicken Burger", Tax = 0.15, Price = 15.99 },
            new Product { Id = "PROD-006", Name = "Coca Cola", Tax = 0.15, Price = 4.50 },
            new Product { Id = "PROD-007", Name = "Fries", Tax = 0.15, Price = 5.99 },
            new Product { Id = "PROD-008", Name = "Onion Rings", Tax = 0.15, Price = 6.99 },
            new Product { Id = "PROD-009", Name = "Milkshake", Tax = 0.15, Price = 7.99 },
            new Product { Id = "PROD-010", Name = "Sprite", Tax = 0.15, Price = 4.50 },
        };

        // Generate ~30 orders spanning back 3 days
        for (int i = 0; i < 30; i++)
        {
            // Random time within the last 3 days (deterministic based on seed)
            var hoursAgo = random.Next(0, 72); // 0 to 72 hours (3 days)
            var minutesAgo = random.Next(0, 60);
            var createdAt = baseDate.AddHours(-hoursAgo).AddMinutes(-minutesAgo);

            // Random number of order lines (1-5 lines per order)
            var lineCount = random.Next(1, 6);
            var lines = new List<OrderLine>();

            for (int j = 0; j < lineCount; j++)
            {
                var product = products[random.Next(products.Count)];
                var quantity = random.Next(1, 6); // 1-5 items
                var discount = random.NextDouble() * 0.3; // 0-30% discount

                lines.Add(new OrderLine
                {
                    Id = $"LINE-{i:D3}-{j:D2}",
                    Quantity = quantity,
                    Product = new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Tax = product.Tax,
                        Price = product.Price
                    },
                    Discount = Math.Round(discount, 2)
                });
            }

            orders.Add(new Order
            {
                Id = $"ORD-{i + 1:D4}",
                CreatedAtUTC = createdAt,
                Lines = lines,
                CurrencyCode = "NZD"
            });
        }

        // Sort by creation date (newest first)
        return orders.OrderByDescending(o => o.CreatedAtUTC).ToList();
    }
}

