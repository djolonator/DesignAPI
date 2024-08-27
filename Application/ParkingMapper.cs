using Infrastracture.Models;
using AutoMapper;
using Domain.Entities;

namespace Application
{
    public class ParkingMapper: Profile
    {
        public ParkingMapper()
        {
            CreateMap<Location, LocationModel>()
            .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Lng, opt => opt.MapFrom(src => src.Longitude))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)).ReverseMap();
        }
    }
}
