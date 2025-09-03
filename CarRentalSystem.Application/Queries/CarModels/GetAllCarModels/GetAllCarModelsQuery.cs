using CarRentalSystem.Application.DTOs.CarModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.CarModels.GetAllCarModels
{
    public record GetAllCarModelsQuery:IRequest<IEnumerable<CarModelDto>>; 

}
