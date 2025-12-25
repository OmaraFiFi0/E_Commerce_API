using AutoMapper;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResponse;
using E_Commerce.Shared.DTOs.AuthenticationDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthenticationService(UserManager<ApplicationUser>userManager,IConfiguration configuration,IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user is null)
                return Error.InvalidCredintals("User.InvalidCredintals");

            var IsPasswordValid =await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!IsPasswordValid)
                return Error.InvalidCredintals("User.InvalidCredintals");

            var token = await CreateTokenAsync(user);
            return new UserDTO(user.Email!, user.DisplayName, token);
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                UserName = registerDTO.UserName,
                PhoneNumber = registerDTO.PhoneNumber,
                
            };
            var IdentityResult = await _userManager.CreateAsync(user,registerDTO.Password);

            if (IdentityResult.Succeeded)
            {
                var token = await CreateTokenAsync(user);
                return new UserDTO(user.Email, user.DisplayName, token);

            }
            return IdentityResult.Errors.Select(E => Error.Validation(E.Code, E.Description)).ToList();
            
        }



        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user != null;
        }

        public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User is null)
            {
                return Error.NotFound("User.NotFound ", $"User With This Email : {email} Was Not Found");
            }
            return new UserDTO(User.Email!, User.DisplayName, await CreateTokenAsync(User));
        }


        public async Task<Result<AddressDTO>> GetUserAddressAsync(string email)
        {
            var User = await _userManager.Users.Include(X=>X.Address).FirstOrDefaultAsync(U=>U.Email == email);
            if(User is null)
                return Error.NotFound("User.NotFound ",  $"User With This Email : {email} Was Not Found");
            
            return _mapper.Map<AddressDTO>(User.Address);
        }

        public async Task<Result<AddressDTO>> UpdateUserAddressAsync(string email,AddressDTO addressDTO)
        {
            var User = await _userManager.Users.Include(X=>X.Address).FirstOrDefaultAsync(U=>U.Email ==email);
            if(User is null)
                return Error.NotFound("User.NotFound ",  $"User With This Email : {email} Was Not Found");

            if(User.Address is not null) // Updated
            {
                User.Address.FirstName = addressDTO.FirstName;
                User.Address.LastName = addressDTO.LastName;
                User.Address.Street = addressDTO.Street;
                User.Address.City = addressDTO.City;
                User.Address.Country = addressDTO.Country;
            }
            else // Created
            {
                User.Address = _mapper.Map<Address>(addressDTO);
            }
            await _userManager.UpdateAsync(User);

            return _mapper.Map<AddressDTO>(User.Address);
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var Claims = new List<Claim>
            {
                new Claim (JwtRegisteredClaimNames.Email , user.Email!),
                new Claim (JwtRegisteredClaimNames.Name , user.UserName!)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
                Claims.Add(new Claim(ClaimTypes.Role , role));
            var SecurityKey = _configuration["JWTOptions:SecurityKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey!));

            var credentials = new SigningCredentials(key , SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                issuer: _configuration["JWTOptions:issuer"],
                audience: _configuration["JWTOptions:audience"],
                expires:DateTime.UtcNow.AddHours(1),
                claims:Claims,
                signingCredentials: credentials
                );

            return  new JwtSecurityTokenHandler().WriteToken(token);
           
        }

        
    }
}
