using AutoMapper;
using StartOfNewPath.BusinessLayer.DTO;
using StartOfNewPath.DataAccessLayer.Entities;

namespace StartOfNewPath.BusinessLayer.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CourseDto, Course>().ReverseMap();
        }
    }
}
