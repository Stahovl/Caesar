using Caesar.Core.Entities;

namespace Caesar.Core.Interfaces;

public interface IOrderRepository
{
    public Task<Order> CreateOrderForReservationAsync(int reservationId, List<int> menuItemIds);
    public Task<Order> GetOrderByReservationIdAsync(int reservationId);
    public Task<IEnumerable<Order>> GetAllOrdersAsync();
    public Task<Order> GetOrderByIdAsync(int orderId);
    public Task UpdateOrderAsync(Order order);
    public Task DeleteOrderAsync(int orderId);
}
