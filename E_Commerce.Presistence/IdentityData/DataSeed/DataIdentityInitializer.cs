using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presistence.IdentityData.DataSeed
{
    public class DataIdentityInitializer:IDataIntializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DataIdentityInitializer> _logger;

        public DataIdentityInitializer(UserManager<ApplicationUser>userManager,
            RoleManager<IdentityRole>roleManager,
            ILogger<DataIdentityInitializer>logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task initializeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if(!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser
                    {
                        DisplayName = "Omar Afifi",
                        UserName = "OmarAfifi",
                        Email="OmarAfifi@gmail.com",
                        PhoneNumber = "01123541041"
                    };

                    var User02 = new ApplicationUser
                    {
                        DisplayName= "Farida Mostafa",
                        UserName= "FaridaMostafa",
                        Email = "FaridaMostafa@gmail.com",
                        PhoneNumber = "01008737572"
                       
                    };

                    await _userManager.CreateAsync(User01,"P@ssw0rd");
                    await _userManager.CreateAsync(User02,"P@ssw0rd");


                    await _userManager.AddToRoleAsync(User01, "SuperAdmin");
                    await _userManager.AddToRoleAsync(User02, "Admin");
                }

            }
            catch (Exception ex)
            {

                _logger.LogError($"Error During Seeding Data ,{ex} happed");
            }
        }
    }
}
