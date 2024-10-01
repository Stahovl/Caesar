using Caesar.Core.Entities;
using Caesar.Core.Interfaces;
using Caesar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Caesar.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly CaesarDbContext _context;

    private readonly IMenuItemRepository _menuItemRepository;

    public OrderRepository(CaesarDbContext context, IMenuItemRepository menuItemRepository)
    {
        _context = context;
        _menuItemRepository = menuItemRepository;
    }

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
            OrderItems = menuItemIds.Select(itemId => new OrderItem
            {
                MenuItemId = itemId,
                Quantity = 1 // Might add logic for quantity
            }).ToList(),
            TotalPrice = menuItems.Sum(item => item.Price)
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<Order> GetOrderByReservationIdAsync(int reservationId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(o => o.ReservationId == reservationId);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task UpdateOrderAsync(Order order)
    {
        _context.Entry(order).State = EntityState.Modified;
        foreach (var item in order.OrderItems)
        {
            _context.Entry(item).State = item.Id == 0 ? EntityState.Added : EntityState.Modified;
        }
        await _context.SaveChangesAsync();
    }


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
