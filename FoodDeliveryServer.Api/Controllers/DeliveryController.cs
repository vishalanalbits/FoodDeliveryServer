using FoodDeliveryServer.Common.Dto.Request;
using FoodDeliveryServer.Common.Dto.Response;
using FoodDeliveryServer.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodDeliveryServer.Api.Controllers
{
    [ApiController]
    [Route("/api")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet("deliveryPersonal")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDeliverys()
        {
            List<DeliveryResponseDto> responseDto = await _deliveryService.GetDeliverys();

            return Ok(responseDto);
        }

        [HttpGet("deliveryPersonal/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDelivery(long id)
        {
            DeliveryResponseDto responseDto = await _deliveryService.GetDelivery(id);

            return Ok(responseDto);
        }

        [HttpPost("deliveryPersonal")]
        public async Task<IActionResult> RegisterDelivery([FromBody] RegisterDelivaryRequestDto requestDto)
        {
            DeliveryResponseDto responseDto = await _deliveryService.RegisterDelivery(requestDto);

            return Ok(responseDto);
        }

        [HttpPut("deliveryPersonal/{id}")]
        [Authorize(Roles = "Delivery")]
        public async Task<IActionResult> UpdateDelivery(long id, [FromBody] UpdateUserRequestDto requestDto)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            long userId = long.Parse(idClaim!.Value);

            if (userId != id)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorResponseDto()
                {
                    Message = "Users can't update information of other users. Access is restricted."
                });
            }

            DeliveryResponseDto responseDto = await _deliveryService.UpdateDelivery(id, requestDto);

            return Ok(responseDto);
        }

        [HttpDelete("deliveryPersonal/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDelivery(long id)
        {
            DeleteEntityResponseDto responseDto = await _deliveryService.DeleteDelivery(id);

            return Ok(responseDto);
        }
    }
}
