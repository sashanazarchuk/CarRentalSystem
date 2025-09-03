using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.DTOs.Cars;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace CarRentalSystem.Application.Commands.Cars.PatchCar
{
    public record PatchCarCommand(int CarId, JsonPatchDocument<PatchCarDto> PatchDoc) :IRequest<CarDto>;
}
