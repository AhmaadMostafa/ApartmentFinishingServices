namespace ApartmentFinishingServices.APIs.Dtos
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<WorkerDto> Workers { get; set; }
    }
}
