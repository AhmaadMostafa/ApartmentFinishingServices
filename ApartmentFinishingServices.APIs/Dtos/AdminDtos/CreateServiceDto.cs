using System.ComponentModel.DataAnnotations;

namespace ApartmentFinishingServices.APIs.Dtos.AdminDtos
{
    public class CreateServiceDto
    {
        [Required]
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }
}
