using Caesar.Core.DTOs;
using Caesar.Core.Entities;
using Caesar.Core.Interfaces;

namespace Caesar.Core.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Order> CreateOrderForReservationAsync(int reservationId, List<int> menuItemIds)
    {
        // Здесь должна быть логика создания заказа
        // Например:
        var order = new Order
        {
            ReservationId = reservationId,
            OrderItems = menuItemIds.Select(id => new OrderItem { MenuItemId = id }).ToList()
        };

        return await _repository.CreateOrderForReservationAsync(reservationId, menuItemIds);
    }

    public async Task<OrderDto> GetOrderByReservationIdAsync(int reservationId)
    {
        var order = await _repository.GetOrderByReservationIdAsync(reservationId);
        return order != null ? MapToDto(order) : null;
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            ReservationId = order.ReservationId,
            OrderItems = order.OrderItems.Select(item => new OrderItemDto
            {
                Id = item.Id,
                MenuItemId = item.MenuItemId,
                Quantity = item.Quantity,
                Price = item.MenuItem.Price
            }).ToList(),
            TotalPrice = order.OrderItems.Sum(item => item.MenuItem.Price * item.Quantity)
        };
    }
}
