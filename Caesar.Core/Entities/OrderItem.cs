namespace Caesar.Core.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int MenuItemId { get; set; }
    public string MenuItemName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
