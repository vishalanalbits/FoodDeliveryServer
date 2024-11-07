using FoodDeliveryServer.Data.Models;

namespace FoodDeliveryServer.Data.Interfaces
{
    public interface IDeliveryRepository
    {
        public Task<bool> IsEmailTaken(string email);
        public Task<bool> IsUsernameTaken(string username);
        public Task<List<Delivery>> GetAllDeliverys();
        public Task<Delivery?> GetDeliveryById(long id);
        public Task<Delivery> RegisterDelivery(Delivery Delivery);
        public Task<Delivery> UpdateDelivery(Delivery Delivery);
        public Task DeleteDelivery(Delivery Delivery);
    }
}
