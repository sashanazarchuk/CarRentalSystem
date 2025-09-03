using AutoMapper;
using CarRentalSystem.Application.DTOs.CarModels;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Validator;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.CarModels.PatchCarModel
{
    internal class PatchCarModelCommandHandler : IRequestHandler<PatchCarModelCommand, CarModelDto>
    {
        private readonly ICarModelRepository _carModelRepository;
        private readonly ICarModelValidator _carModelValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<PatchCarModelCommandHandler> _logger;
        public PatchCarModelCommandHandler(ICarModelRepository carModelRepository, ICarModelValidator carModelValidator, IMapper mapper, ILogger<PatchCarModelCommandHandler> logger)
        {
            _carModelRepository = carModelRepository ?? throw new ArgumentNullException(nameof(carModelRepository));
            _carModelValidator = carModelValidator ?? throw new ArgumentNullException(nameof(carModelValidator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }
        public async Task<CarModelDto> Handle(PatchCarModelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting PATCH for CarModelId {CarId}", request.CarModelId);
           
            var carModel = await _carModelValidator.ValidateCarModelExistsAsync(request.CarModelId, cancellationToken);

            var carModelDto = _mapper.Map<PatchCarModelDto>(carModel);

            _logger.LogDebug("Applying patch document to CarModelId {CarModelId}", request.CarModelId);

            request.PatchDoc.ApplyTo(carModelDto);
            _mapper.Map(carModelDto, carModel);

            await _carModelRepository.PatchAsync(carModel, cancellationToken);

            _logger.LogInformation("CarModel updated in repository successfully");
            return _mapper.Map<CarModelDto>(carModel);
        }
    }
}
