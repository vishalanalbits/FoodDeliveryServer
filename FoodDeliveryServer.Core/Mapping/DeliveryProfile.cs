using AutoMapper;
using FoodDeliveryServer.Common.Dto.Request;
using FoodDeliveryServer.Common.Dto.Response;
using FoodDeliveryServer.Data.Models;

namespace FoodDeliveryServer.Core.Mapping
{
    public class DeliveryProfile : Profile
    {
        public DeliveryProfile()
        {
            CreateMap<RegisterDelivaryRequestDto, Delivery>();

            CreateMap<Delivery, DeliveryResponseDto>();

            CreateMap<UpdateDeliveryRequestDto, Delivery>();

            CreateMap<Delivery, DeleteEntityResponseDto>();
        }
    }
}
