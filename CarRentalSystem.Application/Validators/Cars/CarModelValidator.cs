using CarRentalSystem.Application.DTOs.CarModels;
using CarRentalSystem.Application.Exceptions;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Validator;
using CarRentalSystem.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Validators.Cars
{
    public class CarModelValidator : ICarModelValidator
    {
        private readonly ICarModelRepository _carModelRepository;
        private readonly IBookingRepository _bookingRepository;

        public CarModelValidator(ICarModelRepository carModelRepository, IBookingRepository bookingRepository)
        {
            _carModelRepository = carModelRepository ?? throw new ArgumentNullException(nameof(carModelRepository));
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        }

        public async Task<CarModel> ValidateCarModelExistsAsync(int carModelId, CancellationToken token)
        {
            var carModel = await _carModelRepository.GetByIdAsync(carModelId, token);
            if (carModel == null)
                throw new NotFoundException($"CarModel with ID {carModelId} not found");
            return carModel;
        }

        public async Task ValidateModelIsUniqueAsync(string modelName, int brandId, CancellationToken token)
        {
            var exists = await _carModelRepository.ExistsByNameAsync(modelName, brandId, token);
            if (exists)
                throw new BadRequestException($"Car model '{modelName}' already exists for this brand.");
        }

        public async Task ValidateNoExistingBookingAsync(int carModelId, CancellationToken token)
        {
            var hasBookings = await _bookingRepository.ExistsByCarModelIdAsync(carModelId, token);
            if (hasBookings)
                throw new BadRequestException("Cannot delete CarModel because there are existing bookings for this model.");
        }
    }
}