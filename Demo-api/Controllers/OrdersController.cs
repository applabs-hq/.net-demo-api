using Microsoft.AspNetCore.Mvc;
using Demo_api.Models;
using Demo_api.Repositories;

namespace Demo_api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrdersRepository _ordersRepository;

    public OrdersController(ILogger<OrdersController> logger, IOrdersRepository ordersRepository)
    {
        _logger = logger;
        _ordersRepository = ordersRepository;
    }

    [HttpGet(Name = "GetOrders")]
    public async Task<ActionResult<IEnumerable<Order>>> Get()
    {
        var orders = await _ordersRepository.GetAllOrdersAsync();
        return Ok(orders);
    }
}

