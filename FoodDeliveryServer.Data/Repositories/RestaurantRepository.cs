using FoodDeliveryServer.Data.Contexts;
using FoodDeliveryServer.Data.Interfaces;
using FoodDeliveryServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryServer.Data.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly FoodDeliveryDbContext _dbContext;

        public RestaurantRepository(FoodDeliveryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Restaurant>> GetAllRestaurants()
        {
            return await _dbContext.Restaurants.ToListAsync();
        }

        public async Task<List<Restaurant>> GetRestaurantsByCategory(string category)
        {
            List<Restaurant> allRestaurants = await GetAllRestaurants();

            return allRestaurants.Where(x => x.Category.ToLower() == category.ToLower()).ToList();
        }

        public async Task<List<Restaurant>> GetRestaurantsByCity(string city)
        {
            return await _dbContext.Restaurants.Where(x => x.City.ToLower() == city.ToLower()).ToListAsync();
        }

        public async Task<Restaurant?> GetRestaurantById(long id)
        {
            return await _dbContext.Restaurants.FindAsync(id);
        }

        public async Task<Restaurant> CreateRestaurant(Restaurant restaurant)
        {
            await _dbContext.Restaurants.AddAsync(restaurant);
            await _dbContext.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant> UpdateRestaurant(Restaurant restaurant)
        {
            _dbContext.Entry(restaurant).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return restaurant;
        }

        public async Task DeleteRestaurant(Restaurant restaurant)
        {
            _dbContext.Restaurants.Remove(restaurant);
            await _dbContext.SaveChangesAsync();
            return;
        }
    }
}
