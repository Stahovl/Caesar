using Caesar.Core.DTOs;
using Caesar.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Caesar.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(int reservationId, [FromBody] List<int> menuItemIds)
    {
        _logger.LogInformation($"CreateOrder method called for reservationId: {reservationId} with menu items: {JsonSerializer.Serialize(menuItemIds)}");

        try
        {
            var order = await _orderService.CreateOrderForReservationAsync(reservationId, menuItemIds);

            if (order == null)
            {
                _logger.LogWarning($"Failed to create order for reservationId: {reservationId}");
                return BadRequest(new { message = "Failed to create order." });
            }

            _logger.LogInformation($"Order created successfully for reservationId: {reservationId}");
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while creating order for reservationId: {reservationId}");
            return StatusCode(500, new { message = "An error occurred while creating the order." });
        }
    }

    [HttpGet("{reservationId}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int reservationId)
    {
        var order = await _orderService.GetOrderByReservationIdAsync(reservationId);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }
}
