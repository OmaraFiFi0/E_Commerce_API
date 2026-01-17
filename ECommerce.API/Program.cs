
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Presistence.Data.DataSeed;
using E_Commerce.Presistence.Data.DbContexts;
using E_Commerce.Presistence.IdentityData.DataSeed;
using E_Commerce.Presistence.IdentityData.DbContexts;
using E_Commerce.Presistence.Repositories;
using E_Commerce.Services;
using E_Commerce.Services.MappingProfiles;
using E_Commerce.Services_Abstraction;
using ECommerce.API.CustomMiddleware;
using ECommerce.API.Extensions;
using ECommerce.API.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Register DI Container
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen(); 

            #region Swagger Enhancement
            //
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ECommerceAPI",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = " JWT Authorization Header Using Bearer Schema.Example:\"Authorization:Bearer {Token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,

                });

                options.OperationFilter<SwaggerAuthorizeCheckOperationFilter>();

            });

            #endregion

            builder.Services.AddDbContext<StoreDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddKeyedScoped<IDataIntializer,Datainitialize>("Default");
            builder.Services.AddKeyedScoped<IDataIntializer,DataIdentityInitializer>("Identity");
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(ServiceAssemblyReference).Assembly);
            //builder.Services.AddAutoMapper(X => X.AddProfile<ProductProfile>());
            //builder.Services.AddTransient<ProductPictureUrlResolver>();
            builder.Services.AddScoped<IProductService,ProductService>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });
            builder.Services.AddScoped<IBasketRepository,BasketRepository>();
            builder.Services.AddScoped<IBasketService,BasketService>();
            builder.Services.AddScoped<ICacheRepository, CacheReopsitory>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
            });

            builder.Services.AddDbContext<StoreIdentityDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();
            
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWTOptions:issuer"],
                    ValidAudience = builder.Configuration["JWTOptions:audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecurityKey"]!))
                };
            });
            builder.Services.AddScoped<IOrderService, OrderService>();
            #endregion


            var app = builder.Build();

             await  app.MigrateDataBaseAsync();
            await app.MigrateIdentityDataBaseAsync();

           await  app.SeedDataAsync();
           await app.SeedIdentityDataAsync();

            #region Configure PipeLine [Middlewares]
            // Configure the HTTP request pipeline.

            #region 1st Way To Make Custom Middleware
            //app.Use(async (context, next) =>
            //{
            //    try
            //    {
            //        await next();
            //    }
            //    catch (Exception ex)
            //    {
            //
            //        Console.WriteLine(ex.Message); //Logging Console
            //        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            //        await context.Response.WriteAsJsonAsync(new
            //        {
            //            StatusCode = StatusCodes.Status500InternalServerError,
            //            Error = $"An unexpected error occurred. Please try again later : {ex.Message}"
            //        });
            //    }
            //}); 
            #endregion

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DisplayRequestDuration();
                    options.EnableFilter();
                    //options.DocExpansion(DocExpansion.None);
                });   
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers(); 
            #endregion

           await app.RunAsync();
        }
    }
}
    