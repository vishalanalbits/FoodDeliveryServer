using FoodDeliveryServer.Common.Enums;

namespace FoodDeliveryServer.Common.Dto.Request
{
    public class CreateMenuRequestDto
    {
        public long RestaurantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ItemCategory Category { get; set; }
    }
}
