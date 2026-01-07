using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    public class OrdersController:ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderDataToReturnDTO>>CreateOrder(OrderDTO orderDTO)
        {
            var result = await _orderService.CreateOrderAsync(orderDTO, GetEmailFromToken());
            return HandelResult(result);
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDataToReturnDTO>>> GetOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _orderService.GetAllOrdersAsync(GetEmailFromToken());
            return HandelResult(result);
        }

        [Authorize]
        [HttpGet("{id:guid}")]

        public async Task<ActionResult<OrderDataToReturnDTO>>GetOrder(Guid id)
        {
            var result = await _orderService.GetOrderByIdAsync(id, GetEmailFromToken());
            return HandelResult(result);
        }

        [AllowAnonymous]
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethod()
        {
            var result = await _orderService.GetAllDeliveryMethodAsync();
            return HandelResult(result);
        }
    }
}
