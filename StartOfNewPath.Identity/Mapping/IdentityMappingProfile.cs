using AutoMapper;
using StartOfNewPath.DataAccessLayer.Entities;
using StartOfNewPath.Identity.DTO;

namespace StartOfNewPath.Identity.Mapping
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            CreateMap<RefreshTokenDto, RefreshToken>().ReverseMap();
        }
    }
}
