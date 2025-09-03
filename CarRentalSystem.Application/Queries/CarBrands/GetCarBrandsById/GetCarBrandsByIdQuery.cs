using CarRentalSystem.Application.DTOs.CarBrands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.CarBrands.GetCarBrandsById
{
    public record GetCarBrandsByIdQuery(int CarBrandId):IRequest<CarBrandDto>;
    
}
