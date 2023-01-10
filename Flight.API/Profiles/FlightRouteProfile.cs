using AutoMapper;
using Flight.API.Dtos.Create;
using Flight.API.Dtos.Update;
using Flight.API.Entities;

namespace Flight.API.Profiles
{
    public class FlightRouteProfile : Profile
    {
        public FlightRouteProfile() 
        {
            CreateMap<FlightRouteCreateDto, FlightRoute>()
                .ForMember(dest => dest.DepartureTime, options => options.MapFrom(src => TimeSpan.Parse(src.DepartureTime ?? TimeSpan.Zero.ToString())))
                .ForMember(dest => dest.ArrivalTime, options => options.MapFrom(src => TimeSpan.Parse(src.ArrivalTime ?? TimeSpan.Zero.ToString())));
            CreateMap<FlightRouteUpdateDto, FlightRoute>()
                .ForMember(dest => dest.DepartureTime, options => options.MapFrom(src => TimeSpan.Parse(src.DepartureTime ?? TimeSpan.Zero.ToString())))
                .ForMember(dest => dest.ArrivalTime, options => options.MapFrom(src => TimeSpan.Parse(src.ArrivalTime ?? TimeSpan.Zero.ToString())));
        }
    }
}
