using AutoMapper;
using Flight.API.Dtos.Create;
using Flight.API.Dtos.Update;
using Flight.API.Entities;
using Flight.API.Services.Encrypted;

namespace Flight.API.Profiles
{
    public class DriverProfile : Profile
    {
        public DriverProfile() 
        {
            CreateMap<DriverCreateDto, Driver>();
            CreateMap<DriverUpdateDto, Driver>();
        }
    }
}
