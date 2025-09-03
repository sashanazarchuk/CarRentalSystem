using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Application.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Account.Login
{
    internal class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<LoginUserCommandHandler> _logger;
        public LoginUserCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IOptions<JwtSettings> jwtSettings, ILogger<LoginUserCommandHandler> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _jwtSettings = jwtSettings?.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
            _logger = logger;
        }
        public async Task<TokenDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login attempt for email={Email}", request.Email);

            var user = await _userRepository.FindUserByEmailAsync(request.Email);

            if (user == null || !await _userRepository.ValidatePasswordAsync(user, request.Password))
            {
                _logger.LogWarning("Invalid login attempt for email={Email}", request.Email);
                throw new InvalidLoginException();
            }
            var jwtToken = await _jwtTokenService.CreateAccessToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            
            await _userRepository.UpdateUserAsync(user);
            
            _logger.LogInformation("User {Email} logged in successfully", request.Email);

            return new TokenDto
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken
            };
        }
    }
}
