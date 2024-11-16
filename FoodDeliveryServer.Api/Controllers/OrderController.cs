using FoodDeliveryServer.Common.Dto.Request;
using FoodDeliveryServer.Common.Dto.Response;
using FoodDeliveryServer.Common.Enums;
using FoodDeliveryServer.Core.Interfaces;
using FoodDeliveryServer.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodDeliveryServer.Api.Controllers
{
    [ApiController]
    [Route("/api")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("orders")]
        [Authorize(Roles = "Customer,Partner,Admin,Delivery")]
        public async Task<IActionResult> GetOrders()
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            long userId = long.Parse(idClaim!.Value);

            Claim? roleClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            UserType userType = (UserType)Enum.Parse(typeof(UserType), roleClaim!.Value);

            List<OrderResponseDto> responseDto = await _orderService.GetOrders(userId, userType);

            return Ok(responseDto);
        }

        [HttpPost("orders")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateCheckout([FromBody] OrderRequestDto requestDto)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            long userId = long.Parse(idClaim!.Value);

            CheckoutResponseDto responseDto = await _orderService.CreateCheckout(userId, requestDto);

            return Ok(responseDto);
        }

        [HttpDelete("orders/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RefundOrder(long id)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            long userId = long.Parse(idClaim!.Value);

            DeleteEntityResponseDto responseDto = await _orderService.RefundOrder(id, userId);

            return Ok(responseDto);
        }

        [HttpPatch("orders/partnerstatus/{id}/{orderStatus}")]
        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> PartnerOrderStatus(long id, PartnerOrderStatus orderStatus)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            long userId = long.Parse(idClaim!.Value);
            var status = (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatus.ToString());

            Claim? roleClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            UserType userType = (UserType)Enum.Parse(typeof(UserType), roleClaim!.Value);

            var responseDto = await _orderService.UpdateOrderStatus(id, status, userId, userType);

            return Ok(responseDto);
        }

        [HttpPatch("orders/deliverystatus/{id}/{orderStatus}")]
        [Authorize(Roles = "Delivery")]
        public async Task<IActionResult> DeliveryOrderStatus(long id, DeliveryOrderStatus orderStatus)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            long userId = long.Parse(idClaim!.Value);
            var status = (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatus.ToString());

            Claim? roleClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            UserType userType = (UserType)Enum.Parse(typeof(UserType), roleClaim!.Value);

            var responseDto = await _orderService.UpdateOrderStatus(id, status, userId, userType);

            return Ok(responseDto);
        }

        [HttpPatch("orders/adminstatus/{id}/{orderStatus}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminOrderStatus(long id, AdminOrderStatus orderStatus)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            long userId = long.Parse(idClaim!.Value);
            var status = (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatus.ToString());

            Claim? roleClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            UserType userType = (UserType)Enum.Parse(typeof(UserType), roleClaim!.Value);

            var responseDto = await _orderService.UpdateOrderStatus(id, status, userId, userType);

            return Ok(responseDto);
        }
    }
}
