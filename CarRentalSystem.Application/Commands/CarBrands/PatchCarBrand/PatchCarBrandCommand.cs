using CarRentalSystem.Application.DTOs.CarBrands;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.CarBrands.PatchCarBrand
{
    public record PatchCarBrandCommand(int CarBrandId, JsonPatchDocument<CreateCarBrandDto> PatchDoc): IRequest<CarBrandDto>;

}
