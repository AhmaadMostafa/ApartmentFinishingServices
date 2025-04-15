using ApartmentFinishingServices.APIs.Dtos;
using ApartmentFinishingServices.Core.Entities;
using AutoMapper;

namespace ApartmentFinishingServices.APIs.Helpers
{
    public class CategoryPictureUrlResolver : IValueResolver<Category , CategoryToReturnDto , String>
    {
        private readonly IConfiguration _configuration;

        public CategoryPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Category source , CategoryToReturnDto destination , string destMember , ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]}/{source.PictureUrl}";
            return string.Empty ;
        }
    }
}
