using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.CarModels.DeleteCarModel
{
    public record DeleteCarModelCommand(int CarModelId):IRequest;
    
}
