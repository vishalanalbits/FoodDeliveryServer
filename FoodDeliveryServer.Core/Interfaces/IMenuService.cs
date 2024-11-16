using FoodDeliveryServer.Common.Dto.Request;
using FoodDeliveryServer.Common.Dto.Response;
using FoodDeliveryServer.Common.Enums;

namespace FoodDeliveryServer.Core.Interfaces
{
    public interface IMenuService
    {
        public Task<List<MenuResponseDto>> GetMenus(long? restaurantId, ItemCategory? Category, string? search);
        public Task<MenuResponseDto> GetMenu(long id);
        public Task<MenuResponseDto> CreateMenu(long partnerId, CreateMenuRequestDto requestDto);
        public Task<MenuResponseDto> UpdateMenu(long id, long partnerId, UpdateMenuRequestDto requestDto);
        public Task<DeleteEntityResponseDto> DeleteMenu(long id, long partnerId);
        public Task<ImageResponseDto> UploadImage(long menuId, long partnerId, Stream imageStream, string imageName);
    }
}
