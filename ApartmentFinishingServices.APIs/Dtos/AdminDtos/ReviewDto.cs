namespace ApartmentFinishingServices.APIs.Dtos.AdminDtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } 
        public string? Comment { get; set; }
        public int Rating { get; set; }
    }
}