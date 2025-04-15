using System.ComponentModel.DataAnnotations;

namespace ApartmentFinishingServices.APIs.Dtos
{
    public class RegisterAsWorkerDto : RegisterBaseDto
    {
        public int ServiceId { get; set; }
        public string Description { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public List<AvailableDayDto> AvailableDays { get; set; } = new List<AvailableDayDto>();

    }
}
