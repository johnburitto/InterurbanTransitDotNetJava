using AutoMapper;
using Flight.API.Dtos.Create;
using Flight.API.Dtos.Update;
using Flight.API.Entities;

namespace Flight.API.Profiles
{
    public class FlightEntityProfile : Profile
    {
        public FlightEntityProfile() 
        {
            CreateMap<FlightEntityCreateDto, FlightEntity>();
            CreateMap<FlightEntityUpdateDto, FlightEntity>();
        }
    }
}
