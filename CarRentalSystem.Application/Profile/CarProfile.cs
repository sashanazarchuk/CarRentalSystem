using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.DTOs.Cars;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Profile
{
    public class CarProfile : AutoMapper.Profile
    {
        public CarProfile()
        {
            CreateMap<Car, CarDto>()
            .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Model.ModelName))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Model.Brand.BrandName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => src.FuelType.ToString()))
            .ReverseMap();


            CreateMap<CreateCarDto, Car>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<CarStatus>(src.Status)))
            .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => Enum.Parse<CarFuelType>(src.FuelType)))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ModelId, opt => opt.Ignore())
            .ReverseMap();

            CreateMap<Car, PatchCarDto>()
            .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.ModelId))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => src.FuelType))
            .ForMember(dest => dest.PricePerHour, opt => opt.MapFrom(src => src.PricePerHour))
            .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.ReleaseYear))
            .ReverseMap();

        }
    }
}