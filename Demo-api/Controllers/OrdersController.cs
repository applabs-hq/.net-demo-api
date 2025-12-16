using Microsoft.AspNetCore.Mvc;
using Demo_api.Models;
using Demo_api.Repositories;
using Demo_api.Services;

namespace Demo_api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IOrderService _orderService;

    public OrdersController(ILogger<OrdersController> logger, IOrdersRepository ordersRepository, IOrderService orderService)
    {
        _logger = logger;
        _ordersRepository = ordersRepository;
        _orderService = orderService;
    }

    [HttpGet(Name = "GetOrders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> Get()
    {
        var orders = await _ordersRepository.GetAllOrdersAsync();
        var orderDtos = _orderService.ToDto(orders);
        return Ok(orderDtos);
    }

    [HttpGet("{id}", Name = "GetOrderById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> GetById(string id)
    {
        var order = await _ordersRepository.GetOrderByIdAsync(id);
        
        if (order == null)
        {
            _logger.LogWarning("Order with id {OrderId} not found", id);
            return NotFound($"Order with id {id} not found");
        }

        var orderDto = _orderService.ToDto(order);
        return Ok(orderDto);
    }
}

