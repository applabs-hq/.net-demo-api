namespace Demo_api.Models;

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled,
    Refunded
}

public enum PaymentMethod
{
    CreditCard,
    DebitCard,
    PayPal,
    BankTransfer,
    Cash
}

public class Order
{
    public string Id { get; set; } = string.Empty;
    public DateTime CreatedAtUTC { get; set; }
    public string CurrencyCode { get; set; } = "NZD";
    
    // Customer information
    public string CustomerId { get; set; } = string.Empty;
    
    // Navigation property
    public Customer? Customer { get; set; }
    
    // Shipping information (surplus data)
    public string ShippingAddressLine1 { get; set; } = string.Empty;
    public string ShippingAddressLine2 { get; set; } = string.Empty;
    public string ShippingCity { get; set; } = string.Empty;
    public string ShippingState { get; set; } = string.Empty;
    public string ShippingPostalCode { get; set; } = string.Empty;
    public string ShippingCountry { get; set; } = string.Empty;
    public DateTime? EstimatedDeliveryDate { get; set; }
    public string ShippingMethod { get; set; } = string.Empty;
    public decimal ShippingCost { get; set; }
    public string TrackingNumber { get; set; } = string.Empty;
    
    // Payment information (surplus data)
    public PaymentMethod PaymentMethod { get; set; }
    public string PaymentTransactionId { get; set; } = string.Empty;
    public DateTime? PaymentDate { get; set; }
    public bool IsPaid { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    
    // Order status and metadata (surplus data)
    public OrderStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string InternalNotes { get; set; } = string.Empty;
    public string SalesRepId { get; set; } = string.Empty;
    public string SalesRepName { get; set; } = string.Empty;
    public DateTime? LastModifiedAtUTC { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool IsGift { get; set; }
    public string GiftMessage { get; set; } = string.Empty;
    
    // Navigation property
    public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();
}

