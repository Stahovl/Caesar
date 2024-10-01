using Caesar.Core.DTOs;
using Caesar.Core.Entities;

namespace Caesar.Core.Interfaces;

public interface IOrderService
{
    public Task<Order> CreateOrderForReservationAsync(int reservationId, List<int> menuItemIds);
    public Task<OrderDto> GetOrderByReservationIdAsync(int reservationId);
}
