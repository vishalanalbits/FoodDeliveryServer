using FoodDeliveryServer.Common.Enums;
using FoodDeliveryServer.Data.Contexts;
using FoodDeliveryServer.Data.Interfaces;
using FoodDeliveryServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryServer.Data.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly FoodDeliveryDbContext _dbContext;

        public DeliveryRepository(FoodDeliveryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Delivery>> GetAllDeliverys()
        {
            return await _dbContext.Delivery.ToListAsync();
        }

        public async Task<Delivery?> GetDeliveryById(long id)
        {
            return await _dbContext.Delivery.FindAsync(id);
        }

        public async Task<List<Order>> GetAvailableOrder()
        {
            try
            {
                return await _dbContext.Orders.Where(x => x.Delivery_ID == 0 && x.OrderStatus != OrderStatus.Pending && x.OrderStatus != OrderStatus.Canceled).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Order>();
            }
        }

        public async Task<bool> IsEmailTaken(string email)
        {
            return await _dbContext.Delivery.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> IsUsernameTaken(string username)
        {
            return await _dbContext.Delivery.AnyAsync(x => x.Username == username);
        }

        public async Task<Delivery> RegisterDelivery(Delivery Delivery)
        {
            await _dbContext.Delivery.AddAsync(Delivery);
            await _dbContext.SaveChangesAsync();
            return Delivery;
        }

        public async Task<Delivery> UpdateDelivery(Delivery Delivery)
        {
            _dbContext.Entry(Delivery).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Delivery;
        }

        public async Task DeleteDelivery(Delivery Delivery)
        {
            _dbContext.Delivery.Remove(Delivery);
            await _dbContext.SaveChangesAsync();
            return;
        }
    }
}
