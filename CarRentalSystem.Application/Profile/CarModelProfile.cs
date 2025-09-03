using CarRentalSystem.Application.DTOs.CarModels;
using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Profile
{
    public class CarModelProfile: AutoMapper.Profile
    {
        public CarModelProfile() { 
        
            CreateMap<CarModel,  CarModelDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
                .ReverseMap();

            CreateMap<CarModel, CreateCarModelDto>()
                .ReverseMap();
            
            CreateMap<CarModel, PatchCarModelDto>()
                .ReverseMap();
        }

    }
}
