using CarRentalSystem.Application.DTOs.CarModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.CarModels.GetCarModelsById
{
    public record GetCarModelsByIdQuery(int CarModelId):IRequest<CarModelDto>;


}
