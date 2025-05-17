using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Entities.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Services.Contract
{
    public interface IChatService
    {
        Task<ChatMessage> SendMessageAsync(int requestId, int senderId, int receiverId, string content);
        Task<IReadOnlyList<ChatMessage>> GetMessagesForRequestAsync(int requestId, int userId);
        Task MarkMessagesAsReadAsync(int requestId, int userId);
        Task<int> GetUnreadMessagesCountAsync(int userId);
        Task<AppUser> GetUserAsync(int userId);
        Task<bool> CanUserAccessChatAsync(int requestId, int appUserId);
        Task<RequestParticipantsDto> GetRequestParticipantsAsync(int requestId);
    }
        // New DTO to return participant information
    public class RequestParticipantsDto
    {
        public int RequestId { get; set; }
        public int WorkerId { get; set; }
        public int CustomerId { get; set; }
    }
}