using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Service.Abstracion;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var User = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (User == null)
                return Error.InvalidCrendentials("User.InvalidCrendentials");
            var IsPasswardValid = await _userManager.CheckPasswordAsync(User, loginDTO.Passward);
            if(!IsPasswardValid)
                return Error.InvalidCrendentials("User.InvalidCrendentials");
            return new UserDTO(User.Email, User.DisplayName, "Token");
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser()
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName,
            };
            var IdentityResult = await _userManager.CreateAsync(User, registerDTO.Passward);

            if(IdentityResult.Succeeded)
                return new UserDTO(User.Email, User.DisplayName, "Token");
            return IdentityResult.Errors.Select(E => Error.Validation(E.Code, E.Description)).ToList();

        }
    }
}
