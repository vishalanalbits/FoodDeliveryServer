using FoodDeliveryServer.Common.Enums;

namespace FoodDeliveryServer.Data.Models
{
    public class Partner : User
    {
        public PartnerStatus Status { get; set; }
        public List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
    }
}
