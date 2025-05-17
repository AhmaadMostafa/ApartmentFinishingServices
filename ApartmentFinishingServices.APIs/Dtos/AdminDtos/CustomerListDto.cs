namespace ApartmentFinishingServices.APIs.Dtos.AdminDtos
{
    public class CustomerListDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string Address { get; set; }
        public int? Age { get; set; }
        public string City { get; set; }
        public int? RequestsCount { get; set; }
        public bool IsBlocked { get; set; }


    }
}
