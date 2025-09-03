using CarRentalSystem.Application.Commands.Account.Login;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Application.Settings;
using CarRentalSystem.Domain.Entities;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Tests.UnitTests.Handlers.Account
{
    public class LoginUserCommandHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<LoginUserCommandHandler> _logger;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _jwtTokenService = A.Fake<IJwtTokenService>();
            _jwtSettings = new JwtSettings { RefreshTokenExpirationDays = 7 };
            _logger = A.Fake<ILogger<LoginUserCommandHandler>>();
            _handler = new LoginUserCommandHandler(_userRepository, _jwtTokenService, Options.Create(_jwtSettings), _logger);
        }


        [Fact]
        public async Task Handle_ShouldReturnTokenDto_WhenCredentialsAreValid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123";
            var user = new User { Id = "1", Email = email };

            A.CallTo(() => _userRepository.FindUserByEmailAsync(email)).Returns(Task.FromResult<User?>(user));
            A.CallTo(() => _userRepository.ValidatePasswordAsync(user, password)).Returns(Task.FromResult(true));
            A.CallTo(() => _jwtTokenService.CreateAccessToken(user)).Returns(Task.FromResult("jwt-token"));
            A.CallTo(() => _jwtTokenService.GenerateRefreshToken()).Returns("refresh-token");

            var command = new LoginUserCommand(email, password);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("jwt-token", result.AccessToken);
            Assert.Equal("refresh-token", result.RefreshToken);
            A.CallTo(() => _userRepository.UpdateUserAsync(user)).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task Handle_ShouldThrowInvalidLoginException_WhenUserNotFound()
        {
            // Arrange
            var command = new LoginUserCommand("notfound@example.com", "password");
            A.CallTo(() => _userRepository.FindUserByEmailAsync(command.Email)).Returns(Task.FromResult<User?>(null));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidLoginException>(() => _handler.Handle(command, CancellationToken.None));
        }


        [Fact]
        public async Task Handle_ShouldThrowInvalidLoginException_WhenPasswordInvalid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "wrong-password";
            var user = new User { Id = "1", Email = email };

            A.CallTo(() => _userRepository.FindUserByEmailAsync(email)).Returns(Task.FromResult<User?>(null));
            A.CallTo(() => _userRepository.ValidatePasswordAsync(user, password)).Returns(Task.FromResult(false));

            var command = new LoginUserCommand(email, password);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidLoginException>(() => _handler.Handle(command, CancellationToken.None));
        }

    }
}
