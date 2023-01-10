using AutoMapper;
using Flight.API.Dtos.Create;
using Flight.API.Dtos.Update;
using Flight.API.Entities;

namespace Flight.API.Profiles
{
    public class TransportProfile : Profile
    {
        public TransportProfile() 
        {
            CreateMap<TransportCreateDto, Transport>();
            CreateMap<TransportUpdateDto, Transport>();
        }
    }
}
