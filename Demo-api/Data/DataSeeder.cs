using Demo_api.Models;

namespace Demo_api.Data;

public static class DataSeeder
{
    public static void SeedData(OrdersDbContext context)
    {
        // Check if data already exists
        if (context.Products.Any() || context.Orders.Any())
        {
            return; // Data already seeded
        }

        // Use a seeded random for deterministic results
        var random = new Random(42);
        var baseDate = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);

        // Create products
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

        context.Products.AddRange(products);
        context.SaveChanges();

        // Sample data for seeding
        var customerNames = new[] { "John Smith", "Jane Doe", "Bob Johnson", "Alice Williams", "Charlie Brown", "Diana Prince", "Eve Adams", "Frank Miller" };
        var cities = new[] { "Auckland", "Wellington", "Christchurch", "Hamilton", "Tauranga", "Napier", "Dunedin", "Palmerston North" };
        var shippingMethods = new[] { "Standard", "Express", "Overnight", "Pickup" };
        var salesReps = new[] { ("SREP-001", "Sarah Connor"), ("SREP-002", "Mike Johnson"), ("SREP-003", "Lisa Anderson") };

        // Generate ~30 orders spanning back 3 days
        var orders = new List<Order>();
        for (int i = 0; i < 30; i++)
        {
            // Random time within the last 3 days (deterministic based on seed)
            var hoursAgo = random.Next(0, 72); // 0 to 72 hours (3 days)
            var minutesAgo = random.Next(0, 60);
            var createdAt = baseDate.AddHours(-hoursAgo).AddMinutes(-minutesAgo);

            var customerName = customerNames[random.Next(customerNames.Length)];
            var city = cities[random.Next(cities.Length)];
            var (salesRepId, salesRepName) = salesReps[random.Next(salesReps.Length)];
            var status = (OrderStatus)random.Next(Enum.GetValues(typeof(OrderStatus)).Length);
            var paymentMethod = (PaymentMethod)random.Next(Enum.GetValues(typeof(PaymentMethod)).Length);
            var isPaid = random.NextDouble() > 0.2; // 80% paid
            var isGift = random.NextDouble() > 0.7; // 30% gifts

            var order = new Order
            {
                Id = $"ORD-{i + 1:D4}",
                CreatedAtUTC = createdAt,
                CurrencyCode = "NZD",
                
                // Customer information
                CustomerId = $"CUST-{random.Next(1000, 9999)}",
                CustomerName = customerName,
                CustomerEmail = $"{customerName.ToLower().Replace(" ", ".")}@example.com",
                CustomerPhone = $"+64-{random.Next(20, 30)}-{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                
                // Shipping information
                ShippingAddressLine1 = $"{random.Next(1, 999)} Main Street",
                ShippingAddressLine2 = random.NextDouble() > 0.7 ? $"Unit {random.Next(1, 50)}" : string.Empty,
                ShippingCity = city,
                ShippingState = "NZ",
                ShippingPostalCode = $"{random.Next(1000, 9999)}",
                ShippingCountry = "New Zealand",
                EstimatedDeliveryDate = createdAt.AddDays(random.Next(2, 7)),
                ShippingMethod = shippingMethods[random.Next(shippingMethods.Length)],
                ShippingCost = Math.Round((decimal)(random.NextDouble() * 15 + 5), 2), // $5-$20
                TrackingNumber = random.NextDouble() > 0.3 ? $"TRACK-{random.Next(100000, 999999)}" : string.Empty,
                
                // Payment information
                PaymentMethod = paymentMethod,
                PaymentTransactionId = isPaid ? $"TXN-{random.Next(100000, 999999)}" : string.Empty,
                PaymentDate = isPaid ? createdAt.AddMinutes(random.Next(1, 60)) : null,
                IsPaid = isPaid,
                
                // Order status and metadata
                Status = status,
                Notes = random.NextDouble() > 0.6 ? $"Please deliver to front door" : string.Empty,
                InternalNotes = random.NextDouble() > 0.8 ? $"VIP customer - handle with care" : string.Empty,
                SalesRepId = salesRepId,
                SalesRepName = salesRepName,
                LastModifiedAtUTC = random.NextDouble() > 0.5 ? createdAt.AddMinutes(random.Next(1, 120)) : null,
                ModifiedBy = random.NextDouble() > 0.5 ? salesRepName : string.Empty,
                Priority = random.Next(1, 6), // 1-5
                IsGift = isGift,
                GiftMessage = isGift && random.NextDouble() > 0.5 ? "Happy Birthday!" : string.Empty
            };

            // Random number of order lines (1-5 lines per order)
            var lineCount = random.Next(1, 6);
            var lines = new List<OrderLine>();
            decimal totalAmount = 0;
            decimal taxAmount = 0;
            decimal discountAmount = 0;

            for (int j = 0; j < lineCount; j++)
            {
                var product = products[random.Next(products.Count)];
                var quantity = random.Next(1, 6); // 1-5 items
                var discount = random.NextDouble() * 0.3; // 0-30% discount
                var lineTotal = (decimal)(product.Price * quantity * (1 - discount));
                var lineTax = lineTotal * (decimal)product.Tax;
                var lineDiscount = (decimal)(product.Price * quantity * discount);

                totalAmount += lineTotal;
                taxAmount += lineTax;
                discountAmount += lineDiscount;

                lines.Add(new OrderLine
                {
                    Id = $"LINE-{i:D3}-{j:D2}",
                    Quantity = quantity,
                    ProductId = product.Id,
                    OrderId = order.Id,
                    Discount = Math.Round(discount, 2)
                });
            }

            order.Lines = lines;
            order.TotalAmount = Math.Round(totalAmount + order.ShippingCost, 2);
            order.TaxAmount = Math.Round(taxAmount, 2);
            order.DiscountAmount = Math.Round(discountAmount, 2);
            
            orders.Add(order);
        }

        context.Orders.AddRange(orders);
        context.SaveChanges();
    }
}

