using Caesar.Core.DTOs;
using Caesar.Core.Entities;
using Caesar.Core.Interfaces;

namespace Caesar.Core.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IMenuItemRepository _menuItemRepository; // Чтобы получить информацию о позициях меню
    private readonly IReservationRepository _reservationRepository; // Для проверки существования бронирования

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

        // Получаем все позиции меню, соответствующие переданным идентификаторам
        var menuItems = await _menuItemRepository.GetMenuItemsByIdsAsync(menuItemIds);
        if (menuItems == null || !menuItems.Any())
        {
            throw new Exception("Invalid menu items.");
        }

        // Создаем заказ
        var order = new Order
        {
            ReservationId = reservationId,
            OrderItems = menuItems.Select(menuItem => new OrderItem
            {
                MenuItemId = menuItem.Id,
                Quantity = 1 // Предположим, что по умолчанию количество 1
            }).ToList(),
            TotalPrice = menuItems.Sum(menuItem => menuItem.Price) // Подсчет итоговой цены
        };

        // Сохраняем заказ в базе данных, используя метод репозитория
        await _repository.CreateOrderForReservationAsync(reservationId, menuItemIds);

        // Возвращаем созданный заказ
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
                Price = item.Price // Используем правильную цену
            }).ToList(),
            TotalPrice = order.TotalPrice // Используем TotalPrice из заказа
        };
    }
}
