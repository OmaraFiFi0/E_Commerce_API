using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Specification.OrderSpecifications;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResponse;
using E_Commerce.Shared.DTOs.BasketDTOs;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Forwarding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = E_Commerce.Domain.Entities.ProductModule.Product;

namespace E_Commerce.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PaymentService(IBasketRepository basketRepository 
            , IUnitOfWork unitOfWork 
            , IConfiguration configuration
            , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<Result<BasketDTO>> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            // 0- Check Secret Key  And Assigned To StripeConfiguration.ApiKey
            var Skey = _configuration["Stripe:SKey"];
            if (Skey is null)
                return Error.Faliure("Faild to obtain Secret Key Value ");
            StripeConfiguration.ApiKey = Skey;

            // 1-Retrive The Basket By its Id
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null)
                return Error.NotFound("Basket NotFound");
            // 2-Validation DeliveryMethodId InSide Basket
            if (basket.DeliveryMethodId is null)
                return Error.Validation("DeliveryMethodId Is Not  Found");
            // 3-Retrive The Delivery Method details From The dataBase
            var method = await _unitOfWork.GenericRepository<DeliveryMethod, int>()
                .GetByIdAsync(basket.DeliveryMethodId!.Value);
            // 4-Check if Exsist OR Not 
            if (method is null)
                return Error.NotFound("Delivery Method Not Found ");
            // 5- Assign method.Price ON Our Basket
            basket.ShippingPrice=method.Price;
            // 6- Loop OF Every Iten in baketitem To Check Product Exsist or Not 
                      // And Return The Real Data From DB And Assign in item
            foreach(var item in basket.Items)
            {
                var product = await _unitOfWork.GenericRepository<Product,int>().GetByIdAsync(item.Id);
                if (product is null)
                    return Error.NotFound("ProductItem Was NotFound");
                item.Price = product.Price;
                item.PictureUrl=product.PictureUrl;
                item.ProductName=product.Name;
            }
            long amount = (long)(basket.Items.Sum(I => I.Quantity * I.Price) * 100);
            // 7- Create Or Update Paymentintent With Stripe API
            // basket PaymentIntentId is null ==> Create 
            // basket PaymentIntentId is Exsist ==> Update 

            var stripeService = new PaymentIntentService();
            if(basket.PaymentIntentID is null)//Create
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var paymentIntent = await stripeService.CreateAsync(options);
                basket.PaymentIntentID = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions { Amount = amount };

                await stripeService.UpdateAsync(basket.PaymentIntentID, options);
            }

            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            return _mapper.Map<CustomerBasket,BasketDTO>(basket);
        }

        public async Task UpdateOrderPaymentStatus(string request, string stripeSignature)
        {
            var endPointSecret = _configuration["Stripe:EndpointSecret"];
            var stripeEvent = EventUtility.ConstructEvent(request, stripeSignature, endPointSecret);

            // Handle the event
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            var order = await _unitOfWork
                .GenericRepository<Order, Guid>()
                .GetByIdAsync(new OrderWithPaymentItentIdSpecifications(paymentIntent!.Id));
            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                order.Status = OrderStatus.PaymentRecevied;

                _unitOfWork.GenericRepository<Order, Guid>().Update(order);

                await _unitOfWork.SaveChangesAsync();
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                order.Status = OrderStatus.PaymentFailed;
                _unitOfWork.GenericRepository<Order, Guid>().Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
            // ... handle other event types
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
        }

    }
}
