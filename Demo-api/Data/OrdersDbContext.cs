using Microsoft.EntityFrameworkCore;
using Demo_api.Models;

namespace Demo_api.Data;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Customer entity
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
        });

        // Configure Order entity
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.CurrencyCode).IsRequired().HasMaxLength(3);
            entity.Property(e => e.CreatedAtUTC).IsRequired();
            
            // Customer relationship
            entity.Property(e => e.CustomerId).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Shipping information
            entity.Property(e => e.ShippingAddressLine1).HasMaxLength(200);
            entity.Property(e => e.ShippingAddressLine2).HasMaxLength(200);
            entity.Property(e => e.ShippingCity).HasMaxLength(100);
            entity.Property(e => e.ShippingState).HasMaxLength(100);
            entity.Property(e => e.ShippingPostalCode).HasMaxLength(20);
            entity.Property(e => e.ShippingCountry).HasMaxLength(100);
            entity.Property(e => e.ShippingMethod).HasMaxLength(100);
            entity.Property(e => e.TrackingNumber).HasMaxLength(100);
            
            // Payment information
            entity.Property(e => e.PaymentMethod).HasConversion<string>();
            entity.Property(e => e.PaymentTransactionId).HasMaxLength(200);
            
            // Order status and metadata
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.InternalNotes).HasMaxLength(1000);
            entity.Property(e => e.SalesRepId).HasMaxLength(100);
            entity.Property(e => e.SalesRepName).HasMaxLength(200);
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.GiftMessage).HasMaxLength(500);
        });

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Tax).IsRequired();
            entity.Property(e => e.Price).IsRequired();
        });

        // Configure OrderLine entity
        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.Discount).IsRequired();
            
            // Configure relationships
            entity.HasOne(e => e.Order)
                .WithMany(o => o.Lines)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Product)
                .WithMany(p => p.OrderLines)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

