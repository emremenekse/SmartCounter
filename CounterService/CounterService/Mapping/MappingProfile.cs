using AutoMapper;
using CounterService.DTOs;
using CounterService.Entity;

namespace CounterService.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MeterReading, MeterReadingDTO>().ReverseMap();
        }
    }
}
