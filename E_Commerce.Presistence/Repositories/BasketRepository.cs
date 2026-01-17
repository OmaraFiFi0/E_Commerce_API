using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Presistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default)
        {
           var JsonBasket = JsonSerializer.Serialize(basket);
            var IsCreatedOrUpdatedBasket = await _database.StringSetAsync(basket.Id, JsonBasket, (timeToLive == default) ? TimeSpan.FromDays(7) : timeToLive);
        
               return await GetBasketAsync(basket.Id);
        }

        public Task<bool> DeleteBasket(string basketId)=>_database.KeyDeleteAsync(basketId);
        

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);

            if(basket.IsNullOrEmpty) return null;
            else 
                return JsonSerializer.Deserialize<CustomerBasket?>(basket!);
        }
    }
}
