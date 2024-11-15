using FoodDeliveryServer.Data.Models;

namespace FoodDeliveryServer.Data.Interfaces
{
    public interface IRestaurantRepository
    {
        public Task<List<Restaurant>> GetAllRestaurants();
        public Task<List<Restaurant>> GetRestaurantsByCategory(string category);
        public Task<List<Restaurant>> GetRestaurantsByCity(string city);
        public Task<Restaurant?> GetRestaurantById(long id);
        public Task<Restaurant> CreateRestaurant(Restaurant restaurant);
        public Task<Restaurant> UpdateRestaurant(Restaurant restaurant);
        public Task DeleteRestaurant(Restaurant restaurant);
    }
}
