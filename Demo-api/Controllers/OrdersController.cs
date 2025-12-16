using Microsoft.AspNetCore.Mvc;
using Demo_api.Models;
using Demo_api.Services;

namespace Demo_api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrderService _orderService;

    public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    [HttpGet(Name = "GetOrders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> Get()
    {
        var orderDtos = await _orderService.GetAllOrdersAsync();
        return Ok(orderDtos);
    }

    [HttpGet("{id}", Name = "GetOrderById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> GetById(string id)
    {
        var orderDto = await _orderService.GetOrderByIdAsync(id);
        
        if (orderDto == null)
        {
            _logger.LogWarning("Order with id {OrderId} not found", id);
            return NotFound($"Order with id {id} not found");
        }

        return Ok(orderDto);
    }
}

