using CarRentalSystem.Application.Commands.Account.Registration;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Domain.Entities;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Tests.UnitTests.Handlers.Account
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        

        public RegisterUserCommandHandlerTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _logger = A.Fake<ILogger<RegisterUserCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldRegisterUser_WhenEmailNotExists()
        {
            // Arrange
            var command = new RegisterUserCommand("John Doe", "john@example.com", "1234567890", "Password123");

            A.CallTo(() => _userRepository.IsEmailExistsAsync(command.Email)).Returns(false);

            var identityResult = IdentityResult.Success;
            A.CallTo(() => _userRepository.CreateUserAsync(A<User>._, command.Password))
                .Returns(identityResult);

            var handler = new RegisterUserCommandHandler(_userRepository, _logger);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            A.CallTo(() => _userRepository.AddToRoleAsync(A<User>._, "User")).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task Handle_ShouldReturnFailed_WhenEmailAlreadyExists()
        {
            // Arrange
            var command = new RegisterUserCommand("John Doe", "john@example.com", "1234567890", "Password123");

            A.CallTo(() => _userRepository.IsEmailExistsAsync(command.Email)).Returns(true);

            var handler = new RegisterUserCommandHandler(_userRepository, _logger);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Code == "DuplicateEmail");
            A.CallTo(() => _userRepository.CreateUserAsync(A<User>._, command.Password)).MustNotHaveHappened();
            A.CallTo(() => _userRepository.AddToRoleAsync(A<User>._, "User")).MustNotHaveHappened();
        }


        [Fact]
        public async Task Handle_ShouldReturnFailed_WhenCreateUserFails()
        {
            // Arrange
            var command = new RegisterUserCommand("John Doe", "john@example.com", "1234567890", "Password123");

            A.CallTo(() => _userRepository.IsEmailExistsAsync(command.Email)).Returns(false);

            var identityResult = IdentityResult.Failed(new IdentityError { Code = "RegistrationFailed", Description = "Something went wrong during registration" });
            A.CallTo(() => _userRepository.CreateUserAsync(A<User>._, command.Password))
                .Returns(identityResult);

            var handler = new RegisterUserCommandHandler(_userRepository, _logger);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Code == "RegistrationFailed");
            A.CallTo(() => _userRepository.AddToRoleAsync(A<User>._, "User")).MustNotHaveHappened();
        }

    }
}
