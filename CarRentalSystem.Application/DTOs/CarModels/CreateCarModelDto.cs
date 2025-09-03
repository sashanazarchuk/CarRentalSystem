using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.DTOs.CarModels
{
    public class CreateCarModelDto
    {
        public string ModelName { get; set; } = string.Empty;
        public int BrandId { get; set; }
    }
}
