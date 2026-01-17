using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    public class PaymentsController:ApiBaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("{BasketId}")]
        public async Task<ActionResult<BasketDTO>>CreateOrUpdatePaymentIntent(string BasketId)
        {
            var result = await _paymentService.CreateOrUpdatePaymentIntentAsync(BasketId);
            return HandelResult(result);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var StripeSignature = Request.Headers["Stripe-Signature"];

            await _paymentService.UpdateOrderPaymentStatus(json, StripeSignature!);
            return new EmptyResult();
               
        }
            


    }
}
