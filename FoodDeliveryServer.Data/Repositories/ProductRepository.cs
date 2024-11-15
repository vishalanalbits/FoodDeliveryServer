using FoodDeliveryServer.Common.Enums;
using FoodDeliveryServer.Data.Contexts;
using FoodDeliveryServer.Data.Interfaces;
using FoodDeliveryServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryServer.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly FoodDeliveryDbContext _dbContext;

        public ProductRepository(FoodDeliveryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _dbContext.Products.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByStore(long storeId, ItemCategory? Category, string? search)
        {
            var result = await _dbContext.Products.Where(x => !x.IsDeleted).ToListAsync();
            if (storeId > 0)
            {
                result = result.Where(x => x.StoreId == storeId).ToList();
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

        public async Task<Product?> GetProductById(long id)
        {
            return await _dbContext.Products.Include(x => x.Store).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<Product> CreateProduct(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            _dbContext.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProduct(Product product)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return;
        }
    }
}
