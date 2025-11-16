using AutoMapper;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared.DTOs.ProductDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace E_Commerce.Services.MappingProfiles
{
    internal class ProductPictureUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            
            if (string.IsNullOrEmpty(source.PictureUrl))
                return string.Empty;

            if (source.PictureUrl.StartsWith("http") || source.PictureUrl.StartsWith("https")) 
                return source.PictureUrl;

            //var PictureURL = $"{"https://localhost:7084"}/{source.PictureUrl}";
            var BaseUrl = _configuration.GetSection("URLs")["BaseURL"];

            var PictureURL = $"{BaseUrl}{source.PictureUrl}";

            return PictureURL;

        }
    }
}
