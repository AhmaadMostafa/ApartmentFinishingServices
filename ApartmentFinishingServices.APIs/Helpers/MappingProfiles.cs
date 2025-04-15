using ApartmentFinishingServices.APIs.Dtos;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Entities.Identity;
using AutoMapper;

namespace ApartmentFinishingServices.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryToReturnDto>()
                .ForMember(d => d.PictureUrl, o => o.MapFrom<CategoryPictureUrlResolver>());
            CreateMap<City, CityDto>();
            CreateMap<AppUser, AppUserDto>();
            CreateMap<Worker, WorkerDto>()
                .ForMember(w => w.Name, o => o.MapFrom(p => p.AppUser.Name))
                .ForMember(w => w.City, o => o.MapFrom(p => p.AppUser.City.Name));
            CreateMap<Services, ServiceDto>();
        }
    }
}
