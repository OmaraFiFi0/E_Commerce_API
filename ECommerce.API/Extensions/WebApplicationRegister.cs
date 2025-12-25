using E_Commerce.Domain.Contracts;
using E_Commerce.Presistence.Data.DbContexts;
using E_Commerce.Presistence.IdentityData.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerce.API.Extensions
{
    public static class WebApplicationRegister
    {
        public static async Task<WebApplication> MigrateDataBaseAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var DbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            var PendingMigration = await DbContext.Database.GetPendingMigrationsAsync();
            if (PendingMigration.Any())
            {
                DbContext.Database.Migrate();
            }
            return app;
        }
        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
           await using var scope =  app.Services.CreateAsyncScope();

            var datainitialize = scope.ServiceProvider.GetRequiredKeyedService<IDataIntializer>("Default");

          await  datainitialize.initializeAsync();

            return app;
        }

        public static async Task<WebApplication> SeedIdentityDataAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();

            var datainitialize = scope.ServiceProvider.GetRequiredKeyedService<IDataIntializer>("Identity");

            await datainitialize.initializeAsync();

            return app;
        }
        public static async Task<WebApplication> MigrateIdentityDataBaseAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var DbContext = scope.ServiceProvider.GetRequiredService<StoreIdentityDbContext>();
            var PendingMigration = await DbContext.Database.GetPendingMigrationsAsync();
            if (PendingMigration.Any())
            {
                DbContext.Database.Migrate();
            }
            return app;
        }
    }
}
