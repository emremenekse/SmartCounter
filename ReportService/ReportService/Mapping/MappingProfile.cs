using AutoMapper;
using ReportService.Abstractions;
using ReportService.DTOs;
using ReportService.Entities;

namespace ReportService.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ReportRequest, ReportRequestDTO>().ReverseMap();
            CreateMap<ReportResult, ReportResultDTO>().ReverseMap();
        }
    }
}
