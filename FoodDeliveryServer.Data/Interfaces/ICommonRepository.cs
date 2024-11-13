using FoodDeliveryServer.Common.Enums;

namespace FoodDeliveryServer.Data.Interfaces
{
    public interface ICommonRepository
    {
        Task<int> UpdateUserStatus(long id, bool status, UserType userType);
    }
}
