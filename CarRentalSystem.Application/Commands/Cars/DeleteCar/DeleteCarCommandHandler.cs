using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Validator;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Cars.DeleteCar
{
    internal class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand>
    {
        private readonly ICarRepository _carRepository;
        private readonly ICarValidator _carValidator;
        private readonly ILogger<DeleteCarCommandHandler> _logger;

        public DeleteCarCommandHandler(ICarRepository carRepository, ICarValidator carValidator, ILogger<DeleteCarCommandHandler> logger)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _carValidator = carValidator ?? throw new ArgumentNullException(nameof(carValidator));
            _logger = logger;
        }

        public async Task Handle(DeleteCarCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting delete for CarId {CarId}", request.CarId);
            var car = await _carValidator.ValidateCarExistsAsync(request.CarId, cancellationToken);

            await _carRepository.DeleteAsync(car, cancellationToken);
            _logger.LogInformation("Car with ID {CarId} deleted successfully", request.CarId);
        }
    }
}
