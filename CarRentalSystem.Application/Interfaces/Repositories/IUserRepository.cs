using CarRentalSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> IsEmailExistsAsync(string email);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<User?> FindUserByEmailAsync(string email );
        Task<User?> FindUserByIdAsync(string userId);
        Task<bool> ValidatePasswordAsync(User user, string password);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
    }
}
