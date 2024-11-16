using AutoMapper;
using FoodDeliveryServer.Common.Dto.Response;
using FoodDeliveryServer.Common.Enums;
using FoodDeliveryServer.Common.Exceptions;
using FoodDeliveryServer.Core.Interfaces;
using FoodDeliveryServer.Data.Interfaces;
using FoodDeliveryServer.Data.Models;
using Microsoft.Extensions.Configuration;

namespace FoodDeliveryServer.Core.Services
{
    public class CommonService : ICommonService
    {
        private readonly IConfigurationSection _jwtSettings;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly ICommonRepository _commonRepository;

        public CommonService(IConfiguration config, IAuthRepository authRepository, IMapper mapper, ICommonRepository commonRepository)
        {
            _jwtSettings = config.GetSection("JWTSettings");
            _authRepository = authRepository;
            _mapper = mapper;
            _commonRepository = commonRepository;
        }

        public async Task<bool> UpdateUserStatus(long userId, bool status, UserType userType)
        {
            User? existingUser = await _authRepository.GetUserById(userId, userType);

            if (existingUser == null)
            {
                throw new ResourceNotFoundException("User with this id doesn't exist");
            }            
            var updateStatus = _commonRepository.UpdateUserStatus(userId, status, userType).Result;
            return updateStatus == 1 ? true : false;
        }
    }
}
