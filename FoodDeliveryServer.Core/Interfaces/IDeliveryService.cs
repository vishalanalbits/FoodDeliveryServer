using FoodDeliveryServer.Common.Dto.Request;
using FoodDeliveryServer.Common.Dto.Response;

namespace FoodDeliveryServer.Core.Interfaces
{
    public interface IDeliveryService
    {
        public Task<List<DeliveryResponseDto>> GetDeliverys();
        public Task<DeliveryResponseDto> GetDelivery(long id);
        public Task<List<OrderResponseDto>> GetAvailableOrder();
        public Task<DeliveryResponseDto> RegisterDelivery(RegisterDelivaryRequestDto requestDto);
        public Task<DeliveryResponseDto> UpdateDelivery(long id, UpdateDeliveryRequestDto requestDto);
        public Task<DeleteEntityResponseDto> DeleteDelivery(long id);
    }
}
