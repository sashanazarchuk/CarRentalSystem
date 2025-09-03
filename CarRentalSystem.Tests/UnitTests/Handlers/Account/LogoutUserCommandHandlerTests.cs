using CarRentalSystem.Application.Commands.Account.Logout;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Domain.Entities;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Tests.UnitTests.Handlers.Account
{
    public class LogoutUserCommandHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LogoutUserCommandHandler> _logger;
        private readonly LogoutUserCommandHandler _handler;
        public LogoutUserCommandHandlerTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _logger = A.Fake<ILogger<LogoutUserCommandHandler>>();
            _handler = new LogoutUserCommandHandler(_userRepository, _logger);
        }

        [Fact]
        public async Task Handle_ShouldLogoutUser_WhenUserExists()
        {
            // Arrange
            var userId = "user1";
            var user = new User { Id = userId, RefreshToken = "token", RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1) };

            A.CallTo(() => _userRepository.FindUserByIdAsync(userId))
                .Returns(Task.FromResult<User?>(user));

            var command = new LogoutUserCommand(userId);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(user.RefreshToken);
            Assert.Null(user.RefreshTokenExpiryTime);
            A.CallTo(() => _userRepository.UpdateUserAsync(user))
                .MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "user1";

            A.CallTo(() => _userRepository.FindUserByIdAsync(userId))
                .Returns(Task.FromResult<User?>(null));

            var command = new LogoutUserCommand(userId);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            A.CallTo(() => _userRepository.UpdateUserAsync(A<User>._))
                .MustNotHaveHappened();
        }

    }

}
