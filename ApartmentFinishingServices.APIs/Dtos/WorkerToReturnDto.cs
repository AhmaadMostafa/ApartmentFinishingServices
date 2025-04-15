namespace ApartmentFinishingServices.APIs.Dtos
{
    public class WorkerToReturnDto : UserResponseBaseDto
    {
        public int WorkerId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public List<AvailableDayResponseDto> AvailableDays { get; set; }
    }
}
