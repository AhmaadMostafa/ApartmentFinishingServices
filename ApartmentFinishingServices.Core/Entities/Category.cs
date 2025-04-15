namespace ApartmentFinishingServices.Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? PictureUrl { get; set; }
        public ICollection<Services>? Services { get; set; } = new HashSet<Services>();
    }
}