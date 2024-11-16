using FoodDeliveryServer.Common.Dto.Request;
using FoodDeliveryServer.Common.Dto.Response;

namespace FoodDeliveryServer.Core.Interfaces
{
    public interface IRestaurantService
    {
        public Task<List<RestaurantResponseDto>> GetRestaurants(long? partnerId, double? latitude, double? longitude);
        public Task<RestaurantResponseDto> GetRestaurant(long id);
        public Task<RestaurantResponseDto> CreateRestaurant(long partnerId, RestaurantRequestDto requestDto);
        public Task<RestaurantResponseDto> UpdateRestaurant(long id, long partnerId, RestaurantRequestDto requestDto);
        public Task<DeleteEntityResponseDto> DeleteRestaurant(long id);
        public Task<ImageResponseDto> UploadImage(long RestaurantId, long partnerId, Stream imageStream, string imageName);
    }
}
