using Caesar.Core.DTOs;

namespace Caesar.Core.Interfaces;

public interface IMenuItemService
{
    public Task<MenuItemDto> GetMenuItemAsync(int id);
    public Task<IEnumerable<MenuItemDto>> GetAllMenuItemsAsync();
    public Task<MenuItemDto> CreateMenuItemAsync(MenuItemDto menuItemDto);
    public Task UpdateMenuItemAsync(MenuItemDto menuItemDto);
    public Task DeleteMenuItemAsync(int id);
}
