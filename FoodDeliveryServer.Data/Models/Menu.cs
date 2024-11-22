﻿using FoodDeliveryServer.Common.Enums;

namespace FoodDeliveryServer.Data.Models
{
    public class Menu
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public long RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = default!;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public string? Image { get; set; }
        public string? ImagePublicId { get; set; }
        public ItemCategory Category { get;set; }
    }
}
