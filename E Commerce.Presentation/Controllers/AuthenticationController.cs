using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.AuthenticationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    public class AuthenticationController:ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        // Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>>Login(LoginDTO loginDTO)
        {
            var result = await _authenticationService.LoginAsync(loginDTO);
            return HandelResult(result);
        }

        // Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>>Register(RegisterDTO registerDTO)
        {
            var result = await _authenticationService.RegisterAsync(registerDTO);
            return HandelResult(result);
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>>CheckEmail(string Email)
        {
            var result = await _authenticationService.CheckEmailAsync(Email);

            return Ok(result);

        }
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);


            var Result = await _authenticationService.GetUserByEmailAsync(email!);

            return HandelResult(Result);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddressAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            
            var Result = await _authenticationService.GetUserAddressAsync(email!);

            return HandelResult(Result);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddressAsync(AddressDTO addressDTO)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            
            var Result = await _authenticationService.UpdateUserAddressAsync(email! , addressDTO);

            return HandelResult(Result);
        }
    }
}
