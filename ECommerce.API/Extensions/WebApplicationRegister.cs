using E_Commerce.Domain.Contracts;
using E_Commerce.Presistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Extensions
{
    public static class WebApplicationRegister
    {
        public static WebApplication MigrateDataBase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var DbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            if (DbContext.Database.GetPendingMigrations().Any())
            {
                DbContext.Database.Migrate();
            }
            return app;
        }
        public static WebApplication SeedData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var datainitialize = scope.ServiceProvider.GetRequiredService<IDataIntializer>();

            datainitialize.initialize();

            return app;
        }
    }
}
