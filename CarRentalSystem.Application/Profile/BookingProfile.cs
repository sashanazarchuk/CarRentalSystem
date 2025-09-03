using CarRentalSystem.Application.DTOs.Bookings;
using CarRentalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Profile
{
    public class BookingProfile: AutoMapper.Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.CarStatus, opt => opt.MapFrom(src => src.Car.Status))
                .ReverseMap();

            CreateMap<Booking, CreateBookingDto>()
                .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => src.Car.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap();

            CreateMap<Booking, PatchBookingDto>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ReverseMap();

        }
    }
}
