using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Account.Registration
{
    internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        public RegisterUserCommandHandler(IUserRepository userRepository, ILogger<RegisterUserCommandHandler> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger;
        }
        public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registering user with email={Email}", request.Email);
            
            if (await _userRepository.IsEmailExistsAsync(request.Email))
            {
                _logger.LogWarning("Registration failed: email={Email} already in use", request.Email);
                return IdentityResult.Failed(
                    new IdentityError { Code = "DuplicateEmail", Description = "Email already in use." }
                );
            }

            var user = new User
            {
                FullName = request.FullName,
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userRepository.CreateUserAsync(user, request.Password);

            if (!result.Succeeded)
            {
                _logger.LogWarning("User creation failed for email={Email}: {Errors}", request.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                return result;
            }
            await _userRepository.AddToRoleAsync(user, "User");

            _logger.LogInformation("User registered successfully with email={Email}", request.Email);
            return result;
        }
    }
}