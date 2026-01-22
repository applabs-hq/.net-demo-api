# Technical Interview Tasks - .NET API Developer

## Overview
This document contains the first two tasks for the technical interview. You will be working with an existing .NET 8 Web API that manages orders, customers, products, and order lines.

**Prerequisites:**
- The solution should build and run successfully
- Swagger UI should be accessible at `/swagger`
- Sample data is automatically seeded on application startup

---

## Task 1: Implement Create Order Endpoint

### Objective
Implement a `POST /orders` endpoint that allows creating new orders in the system.

### Requirements

1. **Create Request DTO**
   - Create a `CreateOrderRequest` class with the following properties:
     - `CustomerId` (string, required)
     - `CurrencyCode` (string, optional, defaults to "NZD")
     - `Lines` (collection of `CreateOrderLineRequest`, required, at least one line)
   - Create a `CreateOrderLineRequest` class with:
     - `ProductId` (string, required)
     - `Quantity` (int, required, must be > 0)
     - `Discount` (double, optional, must be between 0 and 1)

2. **Implement POST Endpoint**
   - Add `POST /orders` endpoint to `OrdersController`
   - Accept `CreateOrderRequest` in the request body
   - Return `OrderDto` with HTTP 201 Created status
   - Include `Location` header pointing to the created resource

3. **Business Logic**
   - Validate that the customer exists (return 400 Bad Request if not found)
   - Validate that all products exist (return 400 Bad Request if any product not found)
   - Generate a unique order ID (format: `ORD-XXXX` where XXXX is a 4-digit number)
   - Set `CreatedAtUTC` to current UTC time
   - Calculate order totals:
     - For each line: `lineTotal = (product.Price * quantity) * (1 - discount)`
     - `TaxAmount` = sum of `(lineTotal * product.Tax)` for all lines
     - `DiscountAmount` = sum of `(product.Price * quantity * discount)` for all lines
     - `TotalAmount` = sum of all line totals (shipping cost can be set to 0 for now)
   - Set default values:
     - `Status` = `OrderStatus.Pending`
     - `IsPaid` = false
     - `ShippingCost` = 0

4. **Error Handling**
   - Return 400 Bad Request for validation errors with descriptive messages
   - Return 500 Internal Server Error for unexpected errors
   - Log errors appropriately

5. **Service Layer**
   - Add `CreateOrderAsync(CreateOrderRequest request)` method to `IOrderService` and `OrderService`
   - Add necessary methods to `IOrdersRepository` and `OrdersRepository` if needed

### Expected Behavior

**Success Case:**
```http
POST /orders
Content-Type: application/json

{
  "customerId": "CUST-0001",
  "currencyCode": "NZD",
  "lines": [
    {
      "productId": "PROD-001",
      "quantity": 2,
      "discount": 0.1
    },
    {
      "productId": "PROD-006",
      "quantity": 1,
      "discount": 0
    }
  ]
}
```

**Response:**
- Status: 201 Created
- Location header: `/orders/ORD-0031`
- Body: `OrderDto` with calculated totals

**Error Cases:**
- Invalid customer ID → 400 Bad Request
- Invalid product ID → 400 Bad Request
- Quantity <= 0 → 400 Bad Request
- Discount < 0 or > 1 → 400 Bad Request
- Empty lines array → 400 Bad Request

### Evaluation Criteria
- ✅ Endpoint is properly implemented and follows REST conventions
- ✅ Validation logic is correct and comprehensive
- ✅ Business logic calculations are accurate
- ✅ Error handling is appropriate
- ✅ Code follows existing patterns in the codebase
- ✅ Code is clean, readable, and well-structured

---

## Task 2: Add Filtering and Pagination

### Objective
Extend the existing `GET /orders` endpoint to support filtering by various criteria and pagination.

### Requirements

1. **Query Parameters**
   Add support for the following optional query parameters:
   - `status` (string) - Filter by `OrderStatus` (e.g., "Pending", "Shipped", "Delivered")
   - `customerId` (string) - Filter by customer ID
   - `startDate` (DateTime, ISO 8601 format) - Filter orders created on or after this date
   - `endDate` (DateTime, ISO 8601 format) - Filter orders created on or before this date
   - `page` (int, default: 1) - Page number (1-based)
   - `pageSize` (int, default: 10, max: 100) - Number of items per page

2. **Pagination Response**
   Create a `PagedResponse<T>` class to wrap the results:
   ```csharp
   public class PagedResponse<T>
   {
       public IEnumerable<T> Data { get; set; }
       public int Page { get; set; }
       public int PageSize { get; set; }
       public int TotalCount { get; set; }
       public int TotalPages { get; set; }
       public bool HasPreviousPage { get; set; }
       public bool HasNextPage { get; set; }
   }
   ```

3. **Update GET Endpoint**
   - Modify `GET /orders` to accept query parameters
   - Apply filters to the query
   - Implement pagination
   - Return `PagedResponse<OrderDto>` instead of `IEnumerable<OrderDto>`
   - Maintain existing ordering (by `CreatedAtUTC` descending)

4. **Validation**
   - Validate `page` must be >= 1
   - Validate `pageSize` must be between 1 and 100
   - Validate `startDate` must be before or equal to `endDate` if both provided
   - Validate `status` must be a valid `OrderStatus` enum value
   - Return 400 Bad Request for invalid parameters

5. **Repository/Service Updates**
   - Update `IOrdersRepository` and `OrdersRepository` to support filtering
   - Consider creating a `GetOrdersFilter` or similar class to pass filter criteria
   - Update `IOrderService` and `OrderService` accordingly

### Expected Behavior

**Example Requests:**

```http
GET /orders?page=1&pageSize=5
GET /orders?status=Pending&page=1&pageSize=10
GET /orders?customerId=CUST-0001&startDate=2024-01-14T00:00:00Z&endDate=2024-01-16T23:59:59Z
GET /orders?status=Shipped&page=2&pageSize=20
```

**Example Response:**
```json
{
  "data": [
    {
      "id": "ORD-0030",
      "createdAtUTC": "2024-01-15T10:30:00Z",
      "currencyCode": "NZD",
      "lines": [...]
    },
    ...
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 25,
  "totalPages": 3,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

**Error Cases:**
- Invalid page number (< 1) → 400 Bad Request
- Invalid page size (< 1 or > 100) → 400 Bad Request
- Invalid status value → 400 Bad Request
- startDate > endDate → 400 Bad Request

### Evaluation Criteria
- ✅ All query parameters are properly handled
- ✅ Filtering logic is correct and efficient
- ✅ Pagination is implemented correctly
- ✅ Response format is consistent and informative
- ✅ Validation is comprehensive
- ✅ Query performance is considered (e.g., filtering at database level)
- ✅ Code maintains backward compatibility where possible

