namespace Caesar.Core.Entities;

public class Order
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public decimal TotalPrice { get; set; }
}
