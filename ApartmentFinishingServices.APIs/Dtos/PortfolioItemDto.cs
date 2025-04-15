using System.ComponentModel.DataAnnotations;

namespace ApartmentFinishingServices.APIs.Dtos
{
    public class PortfolioItemDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}