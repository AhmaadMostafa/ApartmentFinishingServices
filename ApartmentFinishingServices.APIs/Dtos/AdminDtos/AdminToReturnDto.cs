namespace ApartmentFinishingServices.APIs.Dtos.AdminDtos
{
    public class AdminToReturnDto : UserResponseBaseDto
    {
        public decimal? TotalEarnings { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalWorkers { get; set; }

    }
}
