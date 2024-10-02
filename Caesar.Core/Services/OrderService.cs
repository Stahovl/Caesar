using Caesar.Core.DTOs;
using Caesar.Core.Entities;
using Caesar.Core.Interfaces;

namespace Caesar.Core.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IReservationRepository _reservationRepository; 

    public OrderService(IOrderRepository repository, IMenuItemRepository menuItemRepository, IReservationRepository reservationRepository)
    {
        _repository = repository;
        _menuItemRepository = menuItemRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<Order> CreateOrderForReservationAsync(int reservationId, List<int> menuItemIds)
    {
        // Проверяем наличие бронирования
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
        {
            throw new Exception($"Reservation with ID {reservationId} not found.");
        }

        // Создаем заказ через репозиторий
        var order = await _repository.CreateOrderForReservationAsync(reservationId, menuItemIds);
        return order;
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
                Price = item.Price 
            }).ToList(),
            TotalPrice = order.TotalPrice 
        };
    }
}
