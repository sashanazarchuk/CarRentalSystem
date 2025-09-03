using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Account.Logout
{
    internal class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LogoutUserCommandHandler> _logger;
        public LogoutUserCommandHandler(IUserRepository userRepository, ILogger<LogoutUserCommandHandler> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger;
        }
        public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Logout attempt by userId={UserId}", request.UserId);
            var user = await _userRepository.FindUserByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("Logout failed: userId={UserId} not found", request.UserId);
                throw new NotFoundException("User not found");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            _logger.LogInformation("UserId={UserId} logged out successfully", request.UserId);
            await _userRepository.UpdateUserAsync(user);

        }
    }
}
