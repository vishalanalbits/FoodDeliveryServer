using FoodDeliveryServer.Common.Enums;
using FoodDeliveryServer.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodDeliveryServer.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;

        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpPatch("UpdateUserStatus/{id}/{userType}/{status}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserStatus(long id, UserType userType, bool status)
        {
            var updateStatus = await _commonService.UpdateUserStatus(id, status, userType);

            return Ok(updateStatus);
        }
    }
}
