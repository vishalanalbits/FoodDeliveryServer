using FoodDeliveryServer.Common.Enums;

namespace FoodDeliveryServer.Core.Interfaces
{
    public interface ICommonService
    {
        Task<bool> UpdateUserStatus(long userId, bool status, UserType userType);
    }
}
