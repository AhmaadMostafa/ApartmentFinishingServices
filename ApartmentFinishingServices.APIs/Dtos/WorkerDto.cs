namespace ApartmentFinishingServices.APIs.Dtos
{
    public class WorkerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int? Rating { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
