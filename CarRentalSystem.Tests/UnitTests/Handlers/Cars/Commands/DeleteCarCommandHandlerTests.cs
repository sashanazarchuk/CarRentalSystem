using CarRentalSystem.Application.Commands.Cars.DeleteCar;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Validator;
using CarRentalSystem.Domain.Entities;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Tests.UnitTests.Handlers.Cars.Commands
{
    public class DeleteCarCommandHandlerTests
    {
        private readonly ICarRepository _carRepository;
        private readonly ICarValidator _carValidator;
        private readonly ILogger<DeleteCarCommandHandler> _logger;
        private readonly DeleteCarCommandHandler _handler;

        public DeleteCarCommandHandlerTests()
        {
            _carRepository = A.Fake<ICarRepository>();
            _carValidator = A.Fake<ICarValidator>();
            _logger = A.Fake<ILogger<DeleteCarCommandHandler>>();
            _handler = new DeleteCarCommandHandler(_carRepository, _carValidator, _logger);
        }

        [Fact]
        public async Task Handle_ShouldDeleteCar_WhenCarExists()
        {
            // Arrange
            var carId = 1;
            var command = new DeleteCarCommand(carId);
            var car = A.Fake<Car>();
            A.CallTo(() => _carValidator.ValidateCarExistsAsync(carId, A<CancellationToken>._)).Returns(Task.FromResult(car));
           
            // Act
            await _handler.Handle(command, CancellationToken.None);
            
            // Assert
            A.CallTo(() => _carRepository.DeleteAsync(car, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }

        
        [Fact]
        public async Task Handle_ShouldThrowException_WhenCarDoesNotExist()
        {
            // Arrange
            var carId = 1;
            var command = new DeleteCarCommand(carId);
            A.CallTo(() => _carValidator.ValidateCarExistsAsync(carId, A<CancellationToken>._))
                .ThrowsAsync(new NotFoundException($"Car with ID {carId} not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            A.CallTo(() => _carRepository.DeleteAsync(A<Car>._, A<CancellationToken>._)).MustNotHaveHappened();
        }
    }
}