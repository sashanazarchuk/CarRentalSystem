using AutoMapper;
using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.Cars.GetCarById
{
    internal class GetCarByIdQueryHandler : IRequestHandler<GetCarByIdQuery, CarDto>
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCarByIdQueryHandler> _logger;
        public GetCarByIdQueryHandler(ICarRepository carRepository, IMapper mapper, ILogger<GetCarByIdQueryHandler> logger)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }
        public async Task<CarDto> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
        {
            var car = await _carRepository.GetByIdAsync(request.id, cancellationToken);

            if (car == null)
            {
                _logger.LogWarning("Car with id {CarId} not found", request.id);
                throw new NotFoundException($"Car with id {request.id} not found");
            }

            var carDto = _mapper.Map<CarDto>(car);
            _logger.LogInformation("Car with id {CarId} successfully retrieved", request.id);
            return carDto;
        }
    }

}