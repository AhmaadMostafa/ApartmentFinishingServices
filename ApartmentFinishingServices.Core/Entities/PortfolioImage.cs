namespace ApartmentFinishingServices.Core.Entities
{
    public class PortfolioImage : BaseEntity
    {
        public string ImgUrl { get; set; }
        public int PortfolioId { get; set; }
        public PortfolioItem PortfolioItem { get; set; }
    }
}