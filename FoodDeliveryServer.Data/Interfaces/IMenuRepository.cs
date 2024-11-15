using FoodDeliveryServer.Common.Enums;
using FoodDeliveryServer.Data.Models;

namespace FoodDeliveryServer.Data.Interfaces
{
    public interface IMenuRepository
    {
        public Task<List<Menu>> GetAllMenus();
        public Task<List<Menu>> GetMenusByRestaurant(long RestaurantId, ItemCategory? Category, string? search);
        public Task<Menu?> GetMenuById(long id);
        public Task<Menu> CreateMenu(Menu menu);
        public Task<Menu> UpdateMenu(Menu menu);
        public Task DeleteMenu(Menu menu);
    }
}
