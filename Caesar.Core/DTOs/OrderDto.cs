namespace Caesar.Core.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public decimal TotalPrice { get; set; }
}
