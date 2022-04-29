using AutoMapper;
using StartOfNewPath.BusinessLayer.DTO;
using StartOfNewPath.Identity.DTO;
using StartOfNewPath.Models;
using StartOfNewPath.Models.Auth;

namespace StartOfNewPath.Mapping
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<CourseModel, CourseDto>().ReverseMap();
            CreateMap<RefreshTokenModel, RefreshTokenDto>().ReverseMap();
        }
    }
}
