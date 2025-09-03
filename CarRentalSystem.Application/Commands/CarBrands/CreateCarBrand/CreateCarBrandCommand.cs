using CarRentalSystem.Application.DTOs.CarBrands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.CarBrands.CreateCarBrand
{
    public record CreateCarBrandCommand():IRequest<CreateCarBrandDto>
    {
        public string BrandName { get; set; } = string.Empty;
    }   

}
