using FoodDeliveryServer.Common.Enums;
using FoodDeliveryServer.Data.Contexts;
using FoodDeliveryServer.Data.Interfaces;
using FoodDeliveryServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryServer.Data.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly FoodDeliveryDbContext _dbContext;

        public MenuRepository(FoodDeliveryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Menu>> GetAllMenus()
        {
            return await _dbContext.Menus.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<List<Menu>> GetMenusByRestaurant(long restaurantId, ItemCategory? Category, string? search)
        {
            var result = await _dbContext.Menus.Where(x => !x.IsDeleted).ToListAsync();
            if (restaurantId > 0)
            {
                result = result.Where(x => x.RestaurantId == restaurantId).ToList();
            }
            if(Category != null)
            {
                result = result.Where(x => x.Category == Category).ToList();
            }
            if (search != null)
            {
                result = result.Where(x => x.Name.Contains(search) || x.Description.Contains(search)).ToList();
            }
            return result;
        }

        public async Task<Menu?> GetMenuById(long id)
        {
            return await _dbContext.Menus.Include(x => x.Restaurant).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<Menu> CreateMenu(Menu menu)
        {
            await _dbContext.Menus.AddAsync(menu);
            await _dbContext.SaveChangesAsync();
            return menu;
        }

        public async Task<Menu> UpdateMenu(Menu menu)
        {
            _dbContext.Entry(menu).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return menu;
        }

        public async Task DeleteMenu(Menu menu)
        {
            _dbContext.Menus.Remove(menu);
            await _dbContext.SaveChangesAsync();
            return;
        }
    }
}
