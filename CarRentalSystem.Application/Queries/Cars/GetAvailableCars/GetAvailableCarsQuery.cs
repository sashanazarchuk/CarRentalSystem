using CarRentalSystem.Application.DTOs.Car;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.Cars.GetAvailableCars
{
    public record GetAvailableCarsQuery : IRequest<List<CarDto>>;

}
