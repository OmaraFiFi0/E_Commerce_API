using AutoMapper;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Shared.DTOs.OrderDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.MappingProfiles
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemsDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemsDTO destination, string destMember, ResolutionContext context)
        {
            if(string.IsNullOrEmpty(source.Product.PictureUrl)) 
                return string.Empty;
            if(source.Product.PictureUrl.StartsWith("http")|| source.Product.PictureUrl.StartsWith("https"))
                 return source.Product.PictureUrl;
            var BaseUrl = _configuration.GetSection("URLs")["BaseURL"];
            if(string.IsNullOrEmpty(BaseUrl) )
                return string.Empty;

            return $"{BaseUrl}{source.Product.PictureUrl}";
        }
    }
}
