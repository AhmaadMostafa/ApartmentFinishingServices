using System.ComponentModel.DataAnnotations;

namespace ApartmentFinishingServices.APIs.Dtos
{
    public class AddReviewDto
    {
        public int WorkerId { get; set; }
        public string? Comment { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]

        public int Rating { get; set; } = 0;
    }
}
