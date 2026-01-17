using AutoMapper;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Shared.DTOs.AuthenticationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.MappingProfiles
{
    public class AuthenticationProfile:Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<Address, AddressDTO>().ReverseMap();
        }
    }
}
