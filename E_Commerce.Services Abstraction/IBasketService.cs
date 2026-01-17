using E_Commerce.Shared.CommonResponse;
using E_Commerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services_Abstraction
{
    public interface IBasketService
    {
        Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO CreateOrUpdateBasket);

        Task<Result<BasketDTO>> GetBasketAsync(string basketId);

        Task<bool> DeleteBasketAsync(string basketId);
    }
}
