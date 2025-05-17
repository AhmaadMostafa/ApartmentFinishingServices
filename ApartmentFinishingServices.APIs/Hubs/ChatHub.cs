using ApartmentFinishingServices.Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ApartmentFinishingServices.APIs.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IRequestService _requestService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IChatService chatService, IRequestService requestService, IHttpContextAccessor httpContextAccessor, ILogger<ChatHub> logger)
        {
            _chatService = chatService;
            _requestService = requestService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation($"User {userId} connected. ConnectionId: {Context.ConnectionId}");

            if (userId != null)
            {
                // Add user to a personal group for direct messages
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
                _logger.LogInformation($"Added user {userId} to group user-{userId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation($"User {userId} disconnected. Reason: {exception?.Message ?? "Normal"}");

            if (userId != null)
            {
                // Remove user from personal group
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{userId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRequestChat(int requestId)
        {
            try
            {
                var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var appUserId))
                {
                    _logger.LogWarning("Failed to get valid user ID in JoinRequestChat");
                    throw new HubException("User is not properly authenticated");
                }

                _logger.LogInformation($"User {appUserId} attempting to join request chat {requestId}");

                // Check if user has access to this chat
                var canAccess = await _chatService.CanUserAccessChatAsync(requestId, appUserId);

                if (!canAccess)
                {
                    _logger.LogWarning($"User {appUserId} denied access to request chat {requestId}");
                    throw new HubException("You don't have permission to access this chat");
                }

                // Add to request-specific group
                await Groups.AddToGroupAsync(Context.ConnectionId, $"request-{requestId}");
                _logger.LogInformation($"User {appUserId} joined request chat {requestId}");

                // Send confirmation - match the case exactly with the client-side handler
                await Clients.Caller.SendAsync("SystemMessage", $"Joined chat for request {requestId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in JoinRequestChat: {ex.Message}");
                throw;
            }
        }

        public async Task SendMessage(int requestId, int receiverId, string content)
        {
            try
            {
                var userIdString = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var senderId))
                {
                    throw new HubException("User not properly authenticated");
                }

                // Validate request and participants - instead of trusting the provided receiverId
                var requestInfo = await _chatService.GetRequestParticipantsAsync(requestId);
                if (requestInfo == null)
                {
                    throw new HubException("Invalid request");
                }

                // Calculate the correct receiver based on the sender
                int correctReceiverId;
                if (senderId == requestInfo.WorkerId)
                {
                    correctReceiverId = requestInfo.CustomerId;
                }
                else if (senderId == requestInfo.CustomerId)
                {
                    correctReceiverId = requestInfo.WorkerId;
                }
                else
                {
                    _logger.LogWarning($"User {senderId} is not a participant in request {requestId}");
                    throw new HubException("You are not a participant in this request");
                }

                // Use the correct receiver ID, not the one provided by the client
                var message = await _chatService.SendMessageAsync(requestId, senderId, correctReceiverId, content);
                if (message == null)
                {
                    throw new HubException("Failed to send message");
                }

                var senderName = (await _chatService.GetUserAsync(senderId))?.Name ?? "Unknown";

                // Create message object
                var messageData = new
                {
                    id = message.Id,
                    requestId = message.RequestId,
                    senderId = message.SenderId,
                    senderName = senderName,
                    receiverId = message.ReceiverId,
                    content = message.Content,
                    sentAt = message.SentAt,
                    isRead = message.IsRead
                };

                // Only send to the specific receiver
                await Clients.Group($"user-{correctReceiverId}").SendAsync("ReceiveMessage", messageData);

                // Send confirmation back to the sender only
                await Clients.Caller.SendAsync("MessageSent", messageData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SendMessage: {ex.Message}");
                throw;
            }
        }

        public async Task MarkMessagesAsRead(int requestId)
        {
            try
            {
                var userIdString = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var appUserId))
                {
                    throw new HubException("User not authenticated properly");
                }

                _logger.LogInformation($"MarkMessagesAsRead - Request: {requestId}, User: {appUserId}");

                // Check if user has access to this chat
                var canAccess = await _chatService.CanUserAccessChatAsync(requestId, appUserId);
                if (!canAccess)
                {
                    _logger.LogWarning($"User {appUserId} denied permission to mark messages as read in request {requestId}");
                    throw new HubException("You don't have permission to access this chat");
                }

                // Mark messages as read
                await _chatService.MarkMessagesAsReadAsync(requestId, appUserId);

                // Get the other participant's ID to notify them
                var requestInfo = await _chatService.GetRequestParticipantsAsync(requestId);
                if (requestInfo != null)
                {
                    int otherUserId = (appUserId == requestInfo.WorkerId) ? requestInfo.CustomerId : requestInfo.WorkerId;

                    // Notify the other participant that messages have been read
                    await Clients.Group($"user-{otherUserId}")
                        .SendAsync("MessagesRead", new
                        {
                            RequestId = requestId,
                            UserId = appUserId,
                            Timestamp = DateTime.UtcNow
                        });
                }

                _logger.LogInformation($"Messages marked as read by user {appUserId} in request {requestId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in MarkMessagesAsRead: {ex.Message}");
                throw;
            }
        }
    }
}