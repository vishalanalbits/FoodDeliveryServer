using FoodDeliveryServer.Data.Contexts;
using FoodDeliveryServer.Data.Interfaces;
using FoodDeliveryServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryServer.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly FoodDeliveryDbContext _dbContext;

        public OrderRepository(FoodDeliveryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _dbContext.Orders.Include(x => x.Restaurant).Include(x => x.Items).ThenInclude(x => x.Menu).ToListAsync();
        }

        public async Task<Order?> GetOrderById(long id)
        {
            return await _dbContext.Orders.Include(x => x.Restaurant).Include(x => x.Items).ThenInclude(x => x.Menu).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Order>> GetOrdersByCustomer(long customerId)
        {
            return await _dbContext.Orders.Include(x => x.Restaurant).Include(x => x.Items).ThenInclude(x => x.Menu).Where(x => x.CustomerId == customerId).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByPartner(long partnerId)
        {
            return await _dbContext.Orders.Include(x => x.Restaurant).Include(x => x.Items).ThenInclude(x => x.Menu).Where(x => x.Restaurant.PartnerId == partnerId).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByDelivery(long deliveryID)
        {
            return await _dbContext.Orders.Where(x => x.Delivery_ID == deliveryID).ToListAsync();
        }

        public async Task<Order> CreateOrder(Order order)
        {
            try
            {
                await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
            return order;
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            _dbContext.Entry(order).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return order;
        }
    }
}
