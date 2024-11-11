using FoodDeliveryServer.Common.Dto.Request;
using FoodDeliveryServer.Common.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryServer.Core.Interfaces
{
    public interface IDeliveryService
    {
        public Task<List<DeliveryResponseDto>> GetDeliverys();
        public Task<DeliveryResponseDto> GetDelivery(long id);
        public Task<DeliveryResponseDto> RegisterDelivery(RegisterDelivaryRequestDto requestDto);
        public Task<DeliveryResponseDto> UpdateDelivery(long id, UpdateUserRequestDto requestDto);
        public Task<DeleteEntityResponseDto> DeleteDelivery(long id);
    }
}
