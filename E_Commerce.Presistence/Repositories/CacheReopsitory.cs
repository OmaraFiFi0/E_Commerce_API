using E_Commerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presistence.Repositories
{
    public class CacheReopsitory : ICacheRepository
    {
        private readonly IDatabase _database;
        public CacheReopsitory(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();   
        }
        public async Task<string?> GetAsync(string cacheKey)
        {
            var cacheValue =  await _database.StringGetAsync(cacheKey);
            return cacheValue.IsNullOrEmpty ? null:cacheValue.ToString();

        }

        public async Task SetAsync(string cacheKey, string cacheValue, TimeSpan timeToLive)
        {
            await _database.StringSetAsync(cacheKey, cacheValue, timeToLive);
        }
    }
}
