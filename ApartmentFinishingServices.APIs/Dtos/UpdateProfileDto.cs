using System.ComponentModel.DataAnnotations;

namespace ApartmentFinishingServices.APIs.Dtos
{
    public class UpdateProfileDto
    {
        public string? Name { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public int? Age { get; set; }
        public int? CityId { get; set; }
        public string? Address { get; set; }

    }
}
