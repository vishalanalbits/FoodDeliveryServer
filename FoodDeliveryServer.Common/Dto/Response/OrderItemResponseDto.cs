namespace FoodDeliveryServer.Common.Dto.Response
{
    public class OrderItemResponseDto
    {
        public long MenuId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public long OrderId { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public decimal MenuPrice { get; set; }
        public string? MenuImage { get; set; }
    }
}
