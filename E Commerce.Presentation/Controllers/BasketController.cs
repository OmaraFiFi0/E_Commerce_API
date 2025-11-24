using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController:ControllerBase 
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        // GET:BaseURL/api/Basket
        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket (string basketId)
        {
            var basket = await _basketService.GetBasketAsync(basketId);
            return Ok(basket);
        }

        // POST:BaseURL/api/Basket
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket (BasketDTO basket)
        {
            var Basket =  await _basketService.CreateOrUpdateBasketAsync(basket);
            return Ok(basket);
        }
        // DELETE:BaseURL/api/Basket/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket ([FromRoute]string id)
        {
            var result = await _basketService.DeleteBasketAsync(id);
            return Ok(result);
        }
    }
}
