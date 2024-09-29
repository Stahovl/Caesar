using Caesar.Core.Entities;

namespace Caesar.Core.Interfaces;

public interface IMenuItemRepository
{
    public Task<MenuItem> GetByIdAsync(int id);
    public Task<List<MenuItem>> GetMenuItemsByIdsAsync(List<int> ids);
    public Task<IEnumerable<MenuItem>> GetAllAsync();
    public Task<MenuItem> AddAsync(MenuItem menuItem);
    public Task UpdateAsync(MenuItem menuItem);
    public Task DeleteAsync(int id);
}