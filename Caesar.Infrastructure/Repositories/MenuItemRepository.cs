using Caesar.Core.Entities;
using Caesar.Core.Interfaces;
using Caesar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Caesar.Infrastructure.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly CaesarDbContext _context;

    public MenuItemRepository(CaesarDbContext context)
    {
        _context = context;
    }

    public async Task<MenuItem> GetByIdAsync(int id)
    {
        return await _context.MenuItems.FindAsync(id);
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync()
    {
        return await _context.MenuItems.ToListAsync();
    }

    public async Task<MenuItem> AddAsync(MenuItem menuItem)
    {
        Console.WriteLine($"Adding MenuItem: {JsonSerializer.Serialize(menuItem)}");
        await _context.MenuItems.AddAsync(menuItem);
        await _context.SaveChangesAsync();
        Console.WriteLine($"MenuItem after save: {JsonSerializer.Serialize(menuItem)}");
        return menuItem;
    }

    public async Task UpdateAsync(MenuItem menuItem)
    {
        _context.Entry(menuItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var menuItem = await _context.MenuItems.FindAsync(id);
        if (menuItem != null)
        {
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
        }
    }
}
