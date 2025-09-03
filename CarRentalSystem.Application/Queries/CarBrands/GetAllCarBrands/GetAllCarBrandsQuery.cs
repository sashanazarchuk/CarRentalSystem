using CarRentalSystem.Application.DTOs.CarBrands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.CarBrands.GetAllCarBrands
{
    public record GetAllCarBrandsQuery:IRequest<IEnumerable<CarBrandDto>>; 

}
