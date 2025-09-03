using CarRentalSystem.Application.DTOs.Cars;
using CarRentalSystem.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.Cars.CreateCar
{
    public record CreateCarCommand:IRequest<CreateCarDto>
    {
        public string Name { get; set; } = string.Empty;
        public int ModelId { get; set; }
        public CarFuelType FuelType { get; set; }
        public decimal PricePerHour { get; set; }
        public int ReleaseYear { get; set; }
    }
}
