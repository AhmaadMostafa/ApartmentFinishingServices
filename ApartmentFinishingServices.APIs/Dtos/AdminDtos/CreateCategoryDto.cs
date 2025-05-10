using System.ComponentModel.DataAnnotations;

namespace ApartmentFinishingServices.APIs.Dtos.AdminDtos
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; }
        public string? PictureUrl { get; set; }

    }
}
