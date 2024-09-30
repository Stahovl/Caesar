﻿namespace Caesar.Core.DTOs;

public class OrderItemDto
{
    public int Id { get; set; }
    public int MenuItemId { get; set; }
    public string MenuItemName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
