﻿namespace ApartmentFinishingServices.APIs.Dtos.ChatDtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
