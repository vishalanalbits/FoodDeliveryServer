﻿using FoodDeliveryServer.Common.Enums;

namespace FoodDeliveryServer.Common.Dto.Response
{
    public class MenuResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public long RestaurantId { get; set; }
        public string? Image { get; set; }
        public ItemCategory Category { get; set; }
    }
}
