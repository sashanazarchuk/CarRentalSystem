using CarRentalSystem.Application.DTOs.CarBrands;
using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Profile
{
    public class CarBrandProfile: AutoMapper.Profile
    {
        public CarBrandProfile()
        {

            CreateMap<CarBrand, CarBrandDto>()
                .ReverseMap();

            CreateMap<CarBrand, CreateCarBrandDto>()
                .ReverseMap();
        }
    }
}