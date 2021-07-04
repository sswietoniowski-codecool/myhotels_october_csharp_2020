using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyHotels.WebApi.Domain;
using MyHotels.WebApi.Models;

namespace MyHotels.WebApi.Configurations.Mapper
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();
        }
    }
}
