using ApartmentFinishingServices.APIs.Dtos;
using ApartmentFinishingServices.APIs.Dtos.AdminDtos;
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
                .ForMember(w => w.City, o => o.MapFrom(p => p.AppUser.City.Name))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.AppUser.ProfilePictureUrl));


            //.ForMember(w => w.Rating, o => o.MapFrom(p => p.Rating));
            CreateMap<City , CityDto>()
                .ForMember(w => w.Name , o => o.MapFrom(p => p.Name));

            CreateMap<Services, ServiceDto>();


            CreateMap<Request, RequestToReturnDto>()
                .ForMember(d => d.RequestId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.WorkerName, o => o.MapFrom(s => s.Worker.AppUser.Name ?? "Unknown Worker"))
                .ForMember(d => d.WorkerId , o => o.MapFrom(s => s.WorkerId))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.AppUser.Name ?? "Unknown Customer"))
                .ForMember(d => d.CustomerAddress, o => o.MapFrom(s => s.Customer.AppUser.Address ?? "No Address Provided"))
                .ForMember(d => d.CustomerCity, o => o.MapFrom(s => s.Customer.AppUser.City.Name ?? "No City Provided"))
                .ForMember(d => d.CustomerphoneNumber, o => o.MapFrom(s => s.Customer.AppUser.PhoneNumber ?? "No Phone Provided"))
                .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service.Name ?? "Unknown Service"))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.NegotiationStatus, o => o.MapFrom(s => s.NegotiationStatus.ToString()));

            CreateMap<Review, ReviewToReturnDto>()
                .ForMember(d => d.WorkerName, o => o.MapFrom(s => s.Worker != null ? s.Worker.AppUser.Name : "Unknown"))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.AppUser.Name : "Unknown"));

            CreateMap<Worker, WorkerListDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppUser.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.AppUser.City.Name))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AppUser.Address))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.AppUser.ProfilePictureUrl))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.AppUser.Age));

            CreateMap<Customer, CustomerListDto>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.AppUser.Name))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.AppUser.Email))
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.AppUser.PhoneNumber))
                .ForMember(d => d.ProfilePictureUrl, o => o.MapFrom(s => s.AppUser.ProfilePictureUrl))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.AppUser.Address))
                .ForMember(d => d.Age, o => o.MapFrom(s => s.AppUser.Age))
                .ForMember(d => d.City, o => o.MapFrom(s => s.AppUser.City.Name));
        }
    }
}
