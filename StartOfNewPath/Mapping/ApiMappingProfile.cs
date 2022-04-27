using AutoMapper;
using StartOfNewPath.BusinessLayer.DTO;
using StartOfNewPath.Models;

namespace StartOfNewPath.Mapping
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<CourseModel, CourseDto>().ReverseMap();
        }
    }
}
