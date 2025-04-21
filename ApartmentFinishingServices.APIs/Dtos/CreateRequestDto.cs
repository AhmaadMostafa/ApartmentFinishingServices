namespace ApartmentFinishingServices.APIs.Dtos
{
    public class CreateRequestDto
    {
        public int WorkerId { get; set; }
        public int ServiceId { get; set; }
        public string? Comment { get; set; }
        public decimal? CustomerSuggestedPrice { get; set; }

    }
}
