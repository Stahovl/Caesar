using Caesar.Core.Entities;
using Caesar.Core.Interfaces;
using Caesar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Caesar.Infrastructure.Repositories;

/// <summary>
/// Repository for managing MenuItem entities in the database.
/// </summary>
public class MenuItemRepository : IMenuItemRepository
{
    private readonly CaesarDbContext _context;

    /// <summary>
    /// Constructor that initializes the database context.
    /// </summary>
    /// <param name="context">The database context.</param>
    public MenuItemRepository(CaesarDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Asynchronously gets a MenuItem entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>The MenuItem entity.</returns>
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
