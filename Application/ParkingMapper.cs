using Application.Models;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ParkingMapper: Profile
    {
        public ParkingMapper()
        {
            CreateMap<Location, LocationModel>()
            .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Lng, opt => opt.MapFrom(src => src.Longitude)).ReverseMap();
        }
    }
}
