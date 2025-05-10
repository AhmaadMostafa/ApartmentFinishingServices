using ApartmentFinishingServices.APIs.Dtos;
using ApartmentFinishingServices.Core.Entities.Identity;
using AutoMapper;

namespace ApartmentFinishingServices.APIs.Helpers
{
    public class ProfilePictureUrlResolver : IValueResolver<AppUser, UserResponseBaseDto, String>
    {
        private readonly IConfiguration _configuration;

        public ProfilePictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(AppUser source, UserResponseBaseDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProfilePictureUrl))
                return $"{_configuration["ApiBaseUrl"]}/{source.ProfilePictureUrl}";
            return string.Empty;
        }
    }
}
