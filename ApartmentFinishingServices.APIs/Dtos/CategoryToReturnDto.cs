namespace ApartmentFinishingServices.APIs.Dtos
{
    public class CategoryToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? PictureUrl { get; set; }
        public List<ServiceDto> Services { get; set; }
    }
}
