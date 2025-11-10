using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
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
        public void initialize()
        {
            try
            {
                var HasProduct = _dbContext.Products.Any();
                var HasBrands = _dbContext.ProductBrands.Any();
                var HasTypes = _dbContext.ProductTypes.Any();

                if (HasProduct && HasBrands && HasTypes) return;

                if (!HasBrands)
                {
                    SeedDataFromJson<ProductBrand,int>("brands.json",_dbContext.ProductBrands); // Save Local
                }

                if (!HasTypes)
                {
                    SeedDataFromJson<ProductType,int>("types.json",_dbContext.ProductTypes); // Save Local
                }
                _dbContext.SaveChanges(); // 1st DataBase Hit

                if (!HasProduct)
                {
                     SeedDataFromJson<Product,int>("products.json",_dbContext.Products); // Save Changes
                }
                _dbContext.SaveChanges(); // 2nd DataBase Hit
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error Occured during data intialization : {ex}");
            }

        }

        private void SeedDataFromJson<T,TKey>(string FileName , DbSet<T> dbset) where T :BaseEntity<TKey>,new() 
        {
            //H:\Route C44\Back End\Eng Khalid Ahmed\ECommerce_Project\ECommerce\E_Commerce.Presistence\Data\DataSeed\JsonFiles\products.json
            // E_Commerce.Presistence\Data\DataSeed\JsonFiles\products.json
            var FilePath = @"..\E_Commerce.Presistence\Data\DataSeed\JsonFiles"+FileName;

            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("JSON File Not Found " , FilePath);
            }

            try
            {
                // When You Wnat To Using Stream
                File.ReadAllText(FilePath);
                var DataStram = File.OpenRead(FilePath);

                var data = JsonSerializer.Deserialize<List<T>>(DataStram,new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if(data is not null )
                {
                    dbset.AddRange(data);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error While Reading Data From JSON File : {ex}");
            }
        }
    }
}
