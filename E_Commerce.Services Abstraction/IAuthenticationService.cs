using E_Commerce.Shared.CommonResponse;
using E_Commerce.Shared.DTOs.AuthenticationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services_Abstraction
{
    public interface IAuthenticationService
    {
        
        // Login 
        Task<Result<UserDTO>>LoginAsync (LoginDTO loginDTO);
        // Register
        Task<Result<UserDTO>>RegisterAsync(RegisterDTO registerDTO);

        //Check Email
        Task<bool> CheckEmailAsync(string email);

        // GetUserByEmailAsync

        Task<Result<UserDTO>>GetUserByEmailAsync(string email);

        Task<Result<AddressDTO>>GetUserAddressAsync(string email);


        Task<Result<AddressDTO>> UpdateUserAddressAsync(string email,AddressDTO address);
        
 
    }
}
