﻿using FoodDeliveryServer.Common.Enums;
using NetTopologySuite.Geometries;

namespace FoodDeliveryServer.Data.Models
{
    public class Order
    {
        public long Id { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime CreatedAt { get; set; } = default!;
        public long CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal ItemsPrice { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal TotalPrice { get; set; }
        public long RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = default!;
        public OrderStatus OrderStatus { get; set; }
        public string Address { get; set; } = string.Empty;
        public Point DeliveryLocation { get; set; } = default!;
        public Coordinate Coordinate { get; set; } = default!;
        public string PaymentIntentId { get; set; } = string.Empty;
        public long Delivery_ID { get; set; } = 0;
    }
}
