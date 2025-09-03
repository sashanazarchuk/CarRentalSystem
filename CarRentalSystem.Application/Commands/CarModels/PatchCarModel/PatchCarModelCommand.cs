using CarRentalSystem.Application.DTOs.CarModels;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.CarModels.PatchCarModel
{
    public record PatchCarModelCommand(int CarModelId, JsonPatchDocument<PatchCarModelDto> PatchDoc) : IRequest<CarModelDto>;
}
