using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Presistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Presistence.Data.DataSeed
{
    public class Datainitialize : IDataIntializer
    {
        private readonly StoreDbContext _dbContext;

        public Datainitialize(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task initializeAsync()
        {
            try
            {
                // DataBase Hits
                var HasProduct = await _dbContext.Products.AnyAsync();
                var HasBrands = await _dbContext.ProductBrands.AnyAsync();
                var HasTypes = await _dbContext.ProductTypes.AnyAsync();
                var HasDeliveryMethod = await _dbContext.Set<DeliveryMethod>().AnyAsync();

                if (HasProduct && HasBrands && HasTypes && HasDeliveryMethod) return;

                if (!HasBrands)
                {
                   await SeedDataFromJson<ProductBrand,int>(FileName: "brands.json",_dbContext.ProductBrands); // Save Local
                }

                if (!HasTypes)
                {
                   await SeedDataFromJson<ProductType,int>("types.json",_dbContext.ProductTypes); // Save Local
                }
                await _dbContext.SaveChangesAsync(); // 1st DataBase Hit

                if (!HasProduct)
                {
                     await SeedDataFromJson<Product,int>("products.json",_dbContext.Products); // Save Local
                }

                if (!HasDeliveryMethod)
                {
                    await SeedDataFromJson<DeliveryMethod,int>("delivery.json",_dbContext.Set<DeliveryMethod>());
                    // Save Local
                }

                await _dbContext.SaveChangesAsync(); // 2nd DataBase Hit

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error Occured during data intialization : {ex}");
            }

        }

        private async Task SeedDataFromJson<T,TKey>(string FileName , DbSet<T> dbset) where T :BaseEntity<TKey>,new() 
        {
            //H:\Route C44\Back End\Eng Khalid Ahmed\ECommerce_Project\ECommerce\E_Commerce.Presistence\Data\DataSeed\JsonFiles\products.json

            // E_Commerce.Presistence\Data\DataSeed\JsonFiles\products.json
            //types.json
            var FilePath = @"..\E_Commerce.Presistence\Data\DataSeed\JsonFiles\" + FileName;

            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("JSON File Not Found " , FilePath);
            }

            try
            {
                // When You Wnat To Using Stream 
                var DataStram = File.OpenRead(FilePath);

                var data = await JsonSerializer.DeserializeAsync<List<T>>(DataStram,new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if(data is not null )
                {
                  await  dbset.AddRangeAsync(data);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error While Reading Data From JSON File : {ex}");
            }
        }
    }
}
