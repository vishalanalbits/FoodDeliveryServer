using FoodDeliveryServer.Common.Enums;
using FoodDeliveryServer.Data.Contexts;
using FoodDeliveryServer.Data.Interfaces;
using FoodDeliveryServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryServer.Data.Repositories
{
    public class CommonRepository : ICommonRepository
    { 
        private readonly FoodDeliveryDbContext _dbContext;

        public CommonRepository(FoodDeliveryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> UpdateUserStatus(long id, bool status, UserType userType)
        {
            switch (userType)
            {
                case UserType.Customer:
                    var customer = await _dbContext.Customers.FindAsync(id);
                    customer.IsActive = status;
                    _dbContext.Entry(customer).State = EntityState.Modified;
                    return await _dbContext.SaveChangesAsync();
                case UserType.Partner:
                    var partner = await _dbContext.Partners.FindAsync(id);
                    partner.IsActive = status;
                    _dbContext.Entry(partner).State = EntityState.Modified;
                    return await _dbContext.SaveChangesAsync();
                case UserType.Admin:
                    var admin = await _dbContext.Admins.FindAsync(id);
                    admin.IsActive = status;
                    _dbContext.Entry(admin).State = EntityState.Modified;
                    return await _dbContext.SaveChangesAsync();
                case UserType.Delivery:
                    var delivery = await _dbContext.Delivery.FindAsync(id);
                    delivery.IsActive = status;
                    _dbContext.Entry(delivery).State = EntityState.Modified;
                    return await _dbContext.SaveChangesAsync();
                default:
                    throw new ArgumentException("Invalid user type");
            }
        }
    }
}
