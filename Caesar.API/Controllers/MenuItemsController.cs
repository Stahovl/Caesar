using Caesar.Core.DTOs;
using Caesar.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Caesar.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MenuItemsController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;
    private readonly ILogger<MenuItemsController> _logger;

    public MenuItemsController(IMenuItemService menuItemService, ILogger<MenuItemsController> logger)
    {
        _menuItemService = menuItemService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItem()
    {
        _logger.LogInformation("GetMenuItem method called");
        if (!User.Identity.IsAuthenticated)
        {
            _logger.LogWarning("Unauthorized access attempt to GetMenuItem");
            return Unauthorized();
        }
        var menuItems = await _menuItemService.GetAllMenuItemsAsync();
        return Ok(menuItems);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MenuItemDto>> GetMenuItem(int id)
    {
        var menuItem = await _menuItemService.GetMenuItemAsync(id);
        if (menuItem == null)
        {
            return NotFound();
        }
        return Ok(menuItem);
    }

    [HttpPost]
    public async Task<ActionResult<MenuItemDto>> CreateMenuItem(MenuItemDto menuItemDto)
    {
        _logger.LogInformation($"CreateMenuItem method called with data: {JsonSerializer.Serialize(menuItemDto)}");
        var createdMenuItem = await _menuItemService.CreateMenuItemAsync(menuItemDto);
        _logger.LogInformation($"Created MenuItem: {JsonSerializer.Serialize(createdMenuItem)}");
        return CreatedAtAction(nameof(GetMenuItem), new { id = createdMenuItem.Id }, createdMenuItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMenuItem(int id, MenuItemDto menuItemDto)
    {
        _logger.LogInformation($"UpdateMenuItem method called for id: {id} with data: {JsonSerializer.Serialize(menuItemDto)}");
        if (id != menuItemDto.Id)
        {
            _logger.LogWarning($"Bad request: id in route ({id}) doesn't match id in body ({menuItemDto.Id})");
            return BadRequest();
        }
        await _menuItemService.UpdateMenuItemAsync(menuItemDto);
        _logger.LogInformation($"Updated MenuItem with id: {id}");
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        _logger.LogInformation($"DeleteMenuItem method called for id: {id}");
        await _menuItemService.DeleteMenuItemAsync(id);
        _logger.LogInformation($"Deleted MenuItem with id: {id}");
        return NoContent();
    }
}
