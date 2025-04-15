namespace ApartmentFinishingServices.APIs.Dtos
{
    public class UserResponseBaseDto
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public int? Age { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string Token { get; set; }
    }
}
