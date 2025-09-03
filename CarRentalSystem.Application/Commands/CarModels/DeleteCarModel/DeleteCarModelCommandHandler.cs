using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Validator;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.CarModels.DeleteCarModel
{
    internal class DeleteCarModelCommandHandler : IRequestHandler<DeleteCarModelCommand>
    {
        private readonly ICarModelRepository _carModelRepository;
        private readonly ICarModelValidator _carModelValidator;
        private readonly ILogger<DeleteCarModelCommandHandler> _logger;
        public DeleteCarModelCommandHandler(ICarModelRepository carModelRepository, ICarModelValidator carModelValidator, ILogger<DeleteCarModelCommandHandler> logger)
        {
            _carModelRepository = carModelRepository ?? throw new ArgumentNullException(nameof(carModelRepository));
            _carModelValidator = carModelValidator ?? throw new ArgumentNullException(nameof(carModelValidator));
            _logger = logger;
        }
        public async Task Handle(DeleteCarModelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting delete for CarModelId {CarModelId}", request.CarModelId);
            
            var carModel = await _carModelValidator.ValidateCarModelExistsAsync(request.CarModelId, cancellationToken);
            await _carModelValidator.ValidateNoExistingBookingAsync(request.CarModelId, cancellationToken);   

            await _carModelRepository.DeleteAsync(carModel, cancellationToken);
            _logger.LogInformation("Car model with ID {CarModelId} deleted successfully", request.CarModelId);
        }
    }
}
