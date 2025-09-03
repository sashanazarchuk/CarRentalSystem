using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Validator;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.CarBrands.DeleteCarBrand
{
    internal class DeleteCarBrandCommandHandler : IRequestHandler<DeleteCarBrandCommand>
    {
        private readonly ICarBrandRepository _carBrandRepository;
        private readonly ICarBrandValidator _carBrandValidator;
        private readonly ILogger<DeleteCarBrandCommandHandler> _logger;
        public DeleteCarBrandCommandHandler(ICarBrandRepository carBrandRepository, ICarBrandValidator carBrandValidator, ILogger<DeleteCarBrandCommandHandler> logger)
        {
            _carBrandRepository = carBrandRepository ?? throw new ArgumentNullException(nameof(carBrandRepository));
            _carBrandValidator = carBrandValidator ?? throw new ArgumentNullException(nameof(carBrandValidator));
            _logger = logger;   
        }
        public async Task Handle(DeleteCarBrandCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting delete for CarBrandId {CarBrandId}", request.CarBrandId);
            
            var carBrand = await _carBrandValidator.ValidateBrandExistsAsync(request.CarBrandId, cancellationToken);
            await _carBrandValidator.ValidateNoExistingCarModelsAsync(request.CarBrandId, cancellationToken);

            await _carBrandRepository.DeleteAsync(carBrand, cancellationToken);
            
            _logger.LogInformation("Car brand with ID {CarBrandId} deleted successfully", request.CarBrandId);
        }
    }
}
