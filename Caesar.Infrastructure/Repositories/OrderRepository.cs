using Caesar.Core.Entities;
using Caesar.Core.Interfaces;
using Caesar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Caesar.Infrastructure.Repositories;

/// <summary>
/// Represents a repository for managing Order entities in the database.
/// This class implements the IOrderRepository interface and provides methods
/// for creating, retrieving, updating, and deleting orders.
/// </summary>
/// <remarks>
/// The OrderRepository uses Entity Framework Core to interact with the database
/// and depends on CaesarDbContext for database operations. It also utilizes
/// IMenuItemRepository for retrieving menu item information.
/// </remarks>
public class OrderRepository : IOrderRepository
{
    private readonly CaesarDbContext _context;

    private readonly IMenuItemRepository _menuItemRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="context">Database context object.</param>
    /// <param name="menuItemRepository">Repository object for work with entities</param>
    public OrderRepository(CaesarDbContext context, IMenuItemRepository menuItemRepository)
    {
        _context = context;
        _menuItemRepository = menuItemRepository;
    }

    /// <summary>
    /// Creates a new order for a given reservation with specified menu items.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation for which to create the order.</param>
    /// <param name="menuItemIds">A list of menu item IDs to be included in the order.</param>
    /// <returns>The created Order object.</returns>
    /// <exception cref="ArgumentException">Thrown when the reservation is not found.</exception>
    public async Task<Order> CreateOrderForReservationAsync(int reservationId, List<int> menuItemIds)
    {
        var reservation = await _context.Reservations.FindAsync(reservationId);
        if (reservation == null)
        {
            throw new ArgumentException("Reservation not found", nameof(reservationId));
        }

        var menuItems = await _menuItemRepository.GetMenuItemsByIdsAsync(menuItemIds);

        var order = new Order
        {
            ReservationId = reservationId,
            OrderItems = menuItems.Select(menuItem => new OrderItem
            {
                MenuItemId = menuItem.Id,
                Quantity = 1,
                Price = menuItem.Price
            }).ToList(),
            TotalPrice = menuItems.Sum(item => item.Price)
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order;
    }

    /// <summary>
    /// Retrieves an order by its associated reservation ID.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation associated with the order.</param>
    /// <returns>The Order object if found; otherwise, null.</returns>
    public async Task<Order> GetOrderByReservationIdAsync(int reservationId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(o => o.ReservationId == reservationId);
    }

    /// <summary>
    /// Retrieves all orders from the database, including their order items and associated menu items.
    /// </summary>
    /// <returns>A collection of all Order objects.</returns>
    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves order by id from the database.
    /// </summary>
    /// <returns>Order objects.</returns>
    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    /// <summary>
    /// Change exist order or create new.
    /// </summary>
    /// <returns>Save change async in order repository.</returns>
    public async Task UpdateOrderAsync(Order order)
    {
        _context.Entry(order).State = EntityState.Modified;
        foreach (var item in order.OrderItems)
        {
            _context.Entry(item).State = item.Id == 0 ? EntityState.Added : EntityState.Modified;
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete order by id from the database.
    /// </summary>
    /// <returns>Save change async in order repository.</returns>
    public async Task DeleteOrderAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null)
        {
            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
