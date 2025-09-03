using AutoMapper;
using CarRentalSystem.Application.DTOs.CarBrands;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Validator;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.CarBrands.PatchCarBrand
{
    internal class PatchCarBrandCommandHandler : IRequestHandler<PatchCarBrandCommand, CarBrandDto>
    {
        private readonly ICarBrandRepository _carBrandRepository;
        private readonly ICarBrandValidator _carBrandValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<PatchCarBrandCommandHandler> _logger;

        public PatchCarBrandCommandHandler(ICarBrandRepository carBrandRepository, ICarBrandValidator carBrandValidator, IMapper mapper, ILogger<PatchCarBrandCommandHandler> logger)
        {
            _carBrandRepository = carBrandRepository ?? throw new ArgumentNullException(nameof(carBrandRepository));
            _carBrandValidator = carBrandValidator ?? throw new ArgumentNullException(nameof(carBrandValidator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        public async Task<CarBrandDto> Handle(PatchCarBrandCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting PATCH for CarBrandId {CarBrandId}", request.CarBrandId);
            var carBrand = await _carBrandValidator.ValidateBrandExistsAsync(request.CarBrandId, cancellationToken);

            var carBrandDto = _mapper.Map<CreateCarBrandDto>(carBrand);

            _logger.LogDebug("Applying patch document to CarBrandId {CarBrandId}", request.CarBrandId);
            
            request.PatchDoc.ApplyTo(carBrandDto);

            _mapper.Map(carBrandDto, carBrand);
            
            await _carBrandRepository.PatchAsync(carBrand, cancellationToken);
            
            _logger.LogInformation("CarBrand updated in repository successfully");
            return _mapper.Map<CarBrandDto>(carBrand);  
        }      
    }
}