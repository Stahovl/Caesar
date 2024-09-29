using Caesar.Core.DTOs;
using Caesar.Core.Entities;

namespace Caesar.Core.Interfaces;

public interface IMenuItemService
{
    public Task<MenuItemDto> GetMenuItemAsync(int id);
    public Task<List<MenuItem>> GetMenuItemsByIdsAsync(List<int> ids);
    public Task<IEnumerable<MenuItemDto>> GetAllMenuItemsAsync();
    public Task<MenuItemDto> CreateMenuItemAsync(MenuItemDto menuItemDto);
    public Task UpdateMenuItemAsync(MenuItemDto menuItemDto);
    public Task DeleteMenuItemAsync(int id);
}
