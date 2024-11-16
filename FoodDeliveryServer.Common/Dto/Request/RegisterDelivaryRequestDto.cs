using FoodDeliveryServer.Common.Dto.Shared;

namespace FoodDeliveryServer.Common.Dto.Request
{
    public class RegisterDelivaryRequestDto : RegisterUserRequestDto
    {
        public bool IsAvailable { get; set; } = false;
        public string Phone { get; set; } = string.Empty;
        public string Vehical { get; set; } = string.Empty;
    }
}
