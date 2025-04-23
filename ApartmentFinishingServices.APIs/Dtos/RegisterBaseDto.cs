using System.ComponentModel.DataAnnotations;

namespace ApartmentFinishingServices.APIs.Dtos
{
    public class RegisterBaseDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public int CityId { get; set; }
        public int? Age { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Address { get; set; }
    }
}
