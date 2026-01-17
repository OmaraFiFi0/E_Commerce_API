using E_Commerce.Shared.CommonResponse;
using E_Commerce.Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services_Abstraction
{
    public interface IOrderService
    {
        // Create Order (OrderDTO , String Email) ==> OrderToReturnDTO
        Task<Result<OrderDataToReturnDTO>>CreateOrderAsync(OrderDTO orderDTO,string Email);

        // GetAllDeliveryMethodAsync
        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethodAsync();

        // GetAllOrdersAsync
        Task<Result<IEnumerable<OrderDataToReturnDTO>>>GetAllOrdersAsync(string email);

        // GetOrderByIdForUserAsync 
        Task<Result<OrderDataToReturnDTO>> GetOrderByIdAsync( Guid Id,string email);
    }
}
