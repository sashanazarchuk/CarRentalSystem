using CarRentalSystem.Application.DTOs.CarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.DTOs.CarBrands
{
    public class CarBrandDto
    {
        public int Id { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public List<CarModelDto> CarModels { get; set; } = new List<CarModelDto>();
    }
}
