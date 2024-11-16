using AutoMapper;
using FoodDeliveryServer.Common.Dto.Request;
using FoodDeliveryServer.Common.Dto.Response;
using FoodDeliveryServer.Data.Models;

namespace FoodDeliveryServer.Core.Mapping
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<CreateMenuRequestDto, Menu>();

            CreateMap<Menu, MenuResponseDto>();

            CreateMap<UpdateMenuRequestDto, Menu>();

            CreateMap<Menu, DeleteEntityResponseDto>();

            CreateMap<Menu, ImageResponseDto>();
        }
    }
}
