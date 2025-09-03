using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Validator;
using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Validators.Cars
{
    public class CarValidator : ICarValidator
    {
        private readonly ICarRepository _carRepository;
        private readonly ICarModelRepository _carModelRepository;

        public CarValidator(ICarRepository carRepository, ICarModelRepository carModelRepository)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _carModelRepository = carModelRepository ?? throw new ArgumentNullException(nameof(carModelRepository));
        }

        public async Task<Car> ValidateCarExistsAsync(int carId, CancellationToken token)
        {
            var car = await _carRepository.GetByIdAsync(carId, token);
            if (car == null)
                throw new NotFoundException($"Car with ID {carId} not found.");

            return car;
        }

        public async Task<CarModel> ValidateCarModelExistsAsync(int modelId, CancellationToken token)
        {
            var carModel = await _carModelRepository.GetByIdWithBrandAsync(modelId, token);
            if (carModel == null)
                throw new NotFoundException($"Car model with ID {modelId} does not exist.");

            return carModel;
        }
    }
}