using Microsoft.AspNetCore.Mvc;
using Testt.DTOs;
using Testt.Exceptions;
using Testt.Services;

namespace Testt.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersService _ordersService;

    public OrdersController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        try
        {
            var order = await _ordersService.GetOrderByIdAsync(id);
            return Ok(order);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id}/fulfill")]
    public async Task<IActionResult> FulfillOrder(int id, FulfillOrderDto dto)
    {
        try
        {
            await _ordersService.FulfillOrderAsync(id, dto);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }
}
