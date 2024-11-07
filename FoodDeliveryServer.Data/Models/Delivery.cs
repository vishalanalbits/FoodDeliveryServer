namespace FoodDeliveryServer.Data.Models
{
    public class Delivery : User
    {
        public bool IsAvailable { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Vehical { get; set; } = string.Empty;
    }
}
