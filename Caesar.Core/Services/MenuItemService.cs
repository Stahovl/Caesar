using Caesar.Core.DTOs;
using Caesar.Core.Entities;
using Caesar.Core.Interfaces;
using System.Text.Json;

namespace Caesar.Core.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _repository;

    public MenuItemService(IMenuItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<MenuItemDto> GetMenuItemAsync(int id)
    {
        var menuItem = await _repository.GetByIdAsync(id);
        return menuItem != null ? MapToDto(menuItem) : null;
    }

    public async Task<List<MenuItem>> GetMenuItemsByIdsAsync(List<int> ids)
    {
        return await _repository.GetMenuItemsByIdsAsync(ids);
    }

    public async Task<IEnumerable<MenuItemDto>> GetAllMenuItemsAsync()
    {
        var menuItems = await _repository.GetAllAsync();
        return menuItems.Select(MapToDto);
    }

    public async Task<MenuItemDto> CreateMenuItemAsync(MenuItemDto menuItemDto)
    {
      /*  var menuItem = MapToEntity(menuItemDto);
        var createdMenuItem = await _repository.AddAsync(menuItem);
        return MapToDto(createdMenuItem);*/

        Console.WriteLine($"Creating MenuItem: {JsonSerializer.Serialize(menuItemDto)}");
        var menuItem = MapToEntity(menuItemDto);
        Console.WriteLine($"Mapped MenuItem: {JsonSerializer.Serialize(menuItem)}");
        var createdMenuItem = await _repository.AddAsync(menuItem);
        Console.WriteLine($"Created MenuItem: {JsonSerializer.Serialize(createdMenuItem)}");
        return MapToDto(createdMenuItem);
    }

    public async Task UpdateMenuItemAsync(MenuItemDto menuItemDto)
    {
        var menuItem = MapToEntity(menuItemDto);
        await _repository.UpdateAsync(menuItem);
    }

    public async Task DeleteMenuItemAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static MenuItemDto MapToDto(MenuItem menuItem)
    {
        return new MenuItemDto
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Description = menuItem.Description,
            Price = menuItem.Price,
            Category = menuItem.Category,
            ImageUrl = menuItem.ImageUrl
        };
    }

    private static MenuItem MapToEntity(MenuItemDto menuItemDto)
    {
        return new MenuItem
        {
            Id = menuItemDto.Id,
            Name = menuItemDto.Name,
            Description = menuItemDto.Description,
            Price = menuItemDto.Price,
            Category = menuItemDto.Category,
            ImageUrl = menuItemDto.ImageUrl
        };
    }
}
