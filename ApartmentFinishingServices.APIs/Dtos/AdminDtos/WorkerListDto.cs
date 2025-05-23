﻿namespace ApartmentFinishingServices.APIs.Dtos.AdminDtos
{
    public class WorkerListDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public string ServiceName { get; set; }
        public int? Rating { get; set; }
        public string Description { get; set; }
        public int? CompletedRequests { get; set; } = 0;
        public bool IsBlocked { get; set; }
        public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();


    }
}
