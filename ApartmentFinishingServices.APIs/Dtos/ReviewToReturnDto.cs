namespace ApartmentFinishingServices.APIs.Dtos
{
    public class ReviewToReturnDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string WorkerName { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
    }
}
