namespace FoodDeliveryServer.Data.Models
{
    public class OrderItem
    {
        public long MenuId { get; set; }
        public Menu Menu { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public long OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public string MenuName { get; set; } = string.Empty;
        public decimal MenuPrice { get; set; }
        public string? MenuImage { get; set; }
        public string MenuDescription { get; set; } = string.Empty;

    }
}
