using System.ComponentModel.DataAnnotations;

namespace ApartmentFinishingServices.APIs.Dtos.ChatDtos
{
    public class SendMessageDto
    {
        public int RequestId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }

    }
}
