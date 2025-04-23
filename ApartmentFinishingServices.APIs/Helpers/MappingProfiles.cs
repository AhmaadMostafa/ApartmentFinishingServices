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
                //.ForMember(w => w.Rating, o => o.MapFrom(p => p.Rating));
            CreateMap<City , CityDto>()
                .ForMember(w => w.Name , o => o.MapFrom(p => p.Name));

            CreateMap<Services, ServiceDto>();


            CreateMap<Request, RequestToReturnDto>()
                .ForMember(d => d.RequestId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.WorkerName, o => o.MapFrom(s => s.Worker.AppUser.Name ?? "Unknown Worker"))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.AppUser.Name ?? "Unknown Customer"))
            .ForMember(d => d.CustomerAddress, o => o.MapFrom(s => s.Customer.AppUser.Address ?? "No Address Provided"))
                .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service.Name ?? "Unknown Service"))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.NegotiationStatus, o => o.MapFrom(s => s.NegotiationStatus.ToString()));

            CreateMap<Review, ReviewToReturnDto>()
                .ForMember(d => d.WorkerName, o => o.MapFrom(s => s.Worker != null ? s.Worker.AppUser.Name : "Unknown"))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.AppUser.Name : "Unknown"));


        }
    }
}
