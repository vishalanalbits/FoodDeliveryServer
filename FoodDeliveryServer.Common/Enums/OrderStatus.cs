namespace FoodDeliveryServer.Common.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        OrderAccepted = 1,
        Preparing = 2,
        ReadyForDelivery = 3,
        PickedUp = 4,
        OutForDelivery = 5,
        Delivered = 6,
        Canceled = 7,
        Reschedule = 8
    }
}
