using FoodDeliveryServer.Common.Dto.Shared;

namespace FoodDeliveryServer.Common.Dto.Response
{
    public class DeliveryResponseDto : UserResponseDto
    {
        public bool IsAvailable { get; set; } = false;
        public string Phone { get; set; } = string.Empty;
        public string Vehical { get; set; } = string.Empty;
    }
}
