using Caesar.Core.DTOs;
using Caesar.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Caesar.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuItemsController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;

    public MenuItemsController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItem()
    {
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
        var createdMenuItem = await _menuItemService.CreateMenuItemAsync(menuItemDto);
        return CreatedAtAction(nameof(GetMenuItem), new { id = createdMenuItem.Id }, createdMenuItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMenuItem(int id, MenuItemDto menuItemDto)
    {
        if (id != menuItemDto.Id)
        {
            return BadRequest();
        }

        await _menuItemService.UpdateMenuItemAsync(menuItemDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        await _menuItemService.DeleteMenuItemAsync(id);
        return NoContent();
    }
}
