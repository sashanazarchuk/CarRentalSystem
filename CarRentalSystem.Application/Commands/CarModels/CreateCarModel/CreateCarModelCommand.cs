using CarRentalSystem.Application.DTOs.CarModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Commands.CarModels.CreateCarModel
{
    public record CreateCarModelCommand : IRequest<CreateCarModelDto>
    {
        public string ModelName { get; set; } = string.Empty;
        public int BrandId { get; set; }
    }

}
