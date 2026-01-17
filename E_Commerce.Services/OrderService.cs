using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Specification.OrderSpecifications;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResponse;
using E_Commerce.Shared.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IMapper mapper , IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
           _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<OrderDataToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email)
        {
            var OrderAddress = _mapper.Map<AddressDTO,OrderAddress>(orderDTO.ShipToAddress);
            
            var Basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if (Basket is null)
                return Error.NotFound("This Basket Was NotFound",
                    $"The Basket With Id{orderDTO.BasketId} Was NotFound");

            if (Basket.PaymentIntentID is null)
                return Error.Validation("PaymentIntent.NotFound");

            List<OrderItem> OrderItems = new List<OrderItem>();
            
            foreach (var item in Basket.Items)
            {
                var product = await _unitOfWork.GenericRepository<Product, int>().GetByIdAsync(item.Id);

                if (product is null)
                    return Error.NotFound("This Product Was NotFound",
                        $"The Product With Id{item.Id} Was NotFound");
                OrderItems.Add(CreateOrderItem(item,product));
            }
            var deliveryMethod = await _unitOfWork.GenericRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);

            if (deliveryMethod is null)
                return Error.NotFound("This DeliveryMethod Was NotFound",
                                        $"The DeliveryMethod With Id{orderDTO.DeliveryMethodId} Was NotFound");

            var SubTotal = OrderItems.Sum(X => X.Price * X.Quantity);

    
            var orderspec = new OrderWithPaymentItentIdSpecifications(Basket.PaymentIntentID);
            var orderRepo = _unitOfWork.GenericRepository<Order, Guid>();
            var OrderExistWithThisPaymentIntent = await orderRepo.GetByIdAsync(orderspec);
            if (OrderExistWithThisPaymentIntent is not null)
                orderRepo.Delete(OrderExistWithThisPaymentIntent);


            var Order = new Order()
            {
                UserEmail = Email,
                PaymentIntentId = Basket.PaymentIntentID,
                Items = OrderItems,
                Address = OrderAddress,
                DeliveryMethod = deliveryMethod,
                SubTotal = SubTotal,

            };
            await _unitOfWork.GenericRepository<Order,Guid>().AddAsync(Order);
             
            bool result = await _unitOfWork.SaveChangesAsync()>0;
            if (!result)
                return Error.Faliure("Order.Faliure", "There was a Proplem While Creating The Order");

            return _mapper.Map<OrderDataToReturnDTO>(Order);
        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethodAsync()
        {
            var deliveryMethods = await _unitOfWork.GenericRepository<DeliveryMethod,int>().GetAllAsync();
            if (!deliveryMethods.Any())
                return Error.NotFound("DeliveryMethod.NotFound", "No DeliveryMethod Founded");
           var data =  _mapper.Map<IEnumerable<DeliveryMethod>,IEnumerable<DeliveryMethodDTO>>(deliveryMethods);
            if (data is null)
                return Error.NotFound("DeliveryMethod.NotFound", "No DeliveryMethod Founded");
            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(data);
        }

        public async Task<Result<IEnumerable<OrderDataToReturnDTO>>> GetAllOrdersAsync(string email)
        {
            var OrderSpec = new OrderSpecification(email);

            var Orders = await _unitOfWork.GenericRepository<Order, Guid>().GetAllAsync(OrderSpec) ;
            
            if(!Orders.Any())
                return Error.NotFound("Orders.NotFound", $"No Orders Founded For The User With Email:{email}");

            var Data = _mapper.Map<IEnumerable<Order>,IEnumerable<OrderDataToReturnDTO>>(Orders);
            return Result<IEnumerable<OrderDataToReturnDTO>>.Ok(Data);

        }

        public async Task<Result<OrderDataToReturnDTO>> GetOrderByIdAsync(Guid Id, string email)
        {
            var OrderSpec = new OrderSpecification(Id, email);
            var Order = await _unitOfWork.GenericRepository<Order,Guid>().GetByIdAsync(OrderSpec);
            if(Order is null )
                return Error.NotFound("Order.NotFound", $"No Orders Founded With Id:{Id} For The User With Email:{email}");
            var data = _mapper.Map<Order,OrderDataToReturnDTO>(Order);
            return Result<OrderDataToReturnDTO>.Ok(data);
        }

        private OrderItem  CreateOrderItem(BasketItems item, Product product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOrdered()
                {

                    ProductId = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureUrl,

                },
                Price = product.Price,
                Quantity = item.Quantity,
            };
        }
    }
}
