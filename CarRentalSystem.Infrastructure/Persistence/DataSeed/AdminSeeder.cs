using CarRentalSystem.Application.Settings;
using CarRentalSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Persistence.DataSeed
{
    internal class AdminSeeder
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AdminSettings _settings;
        private readonly ILogger<AdminSeeder> _logger;

        public AdminSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<AdminSettings> options, ILogger<AdminSeeder> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger;

        }

        public async Task SeedAdminAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                _logger.LogInformation("Role 'Admin' created.");
            }
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
                _logger.LogInformation("Role 'User' created.");
            }
            
            var admin = await _userManager.FindByEmailAsync(_settings.AdminEmail);
            if (admin == null)
            {
                admin = new User
                {
                    FullName = _settings.AdminName,
                    UserName = _settings.AdminEmail,
                    Email = _settings.AdminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(admin, _settings.AdminPassword);

                if (result.Succeeded)
                {
                    await _userManager.UpdateAsync(admin);
                    await _userManager.AddToRoleAsync(admin, "Admin");
                    _logger.LogInformation("Admin user '{Email}' created.", _settings.AdminEmail);
                }
            }
            else
            {
                _logger.LogInformation("Admin user '{Email}' already exists.", _settings.AdminEmail);
            }
        }
    }
}