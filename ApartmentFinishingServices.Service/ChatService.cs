using ApartmentFinishingServices.Core.Services.Contract;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Entities.Identity;
using ApartmentFinishingServices.Core.Repository.Contract;
using ApartmentFinishingServices.Core.Specifications.Chat_Specs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApartmentFinishingServices.Core.Specifications.Request_specs;
using ApartmentFinishingServices.Core.Specifications;
using ApartmentFinishingServices.Core;

namespace ApartmentFinishingServices.Service
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public ChatService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // Get the worker and customer IDs for a request (new method)
        public async Task<RequestParticipantsDto> GetRequestParticipantsAsync(int requestId)
        {
            try
            {
                var spec = new RequestWithDetailsSpecification(requestId);
                var request = await _unitOfWork.Repository<Request>().GetByIdWithSpec(spec);

                // Modified to allow both Pending and Accepted requests
                if (request == null || (request.Status != RequestStatus.Accepted && request.Status != RequestStatus.Pending))
                {
                    return null;
                }

                return new RequestParticipantsDto
                {
                    RequestId = request.Id,
                    WorkerId = request.Worker.AppUserId,
                    CustomerId = request.Customer.AppUserId
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetRequestParticipantsAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<ChatMessage> SendMessageAsync(int requestId, int senderId, int receiverId, string content)
        {
            try
            {
                // Verify request is accepted or pending
                var requestSpec = new RequestWithDetailsSpecification(requestId);
                var request = await _unitOfWork.Repository<Request>().GetByIdWithSpec(requestSpec);

                // Modified to allow both Pending and Accepted requests
                if (request == null || (request.Status != RequestStatus.Accepted && request.Status != RequestStatus.Pending))
                {
                    Console.WriteLine($"Request {requestId} not found or not in valid status (should be Pending or Accepted)");
                    return null;
                }

                // Verify sender is part of this request
                if (request.Worker.AppUserId != senderId && request.Customer.AppUserId != senderId)
                {
                    Console.WriteLine($"Sender {senderId} is not associated with request {requestId}");
                    return null;
                }

                // Verify receiver is part of this request
                if (request.Worker.AppUserId != receiverId && request.Customer.AppUserId != receiverId)
                {
                    Console.WriteLine($"Receiver {receiverId} is not associated with request {requestId}");
                    return null;
                }

                var message = new ChatMessage
                {
                    RequestId = requestId,
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Content = content,
                    SentAt = DateTime.UtcNow,
                    IsRead = false
                };

                await _unitOfWork.Repository<ChatMessage>().Add(message);
                await _unitOfWork.CompleteAsync();

                return message;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendMessageAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<IReadOnlyList<ChatMessage>> GetMessagesForRequestAsync(int requestId, int userId)
        {
            try
            {
                // First check if the user has access to this chat
                if (!await CanUserAccessChatAsync(requestId, userId))
                {
                    Console.WriteLine($"User {userId} does not have access to request {requestId}");
                    return Array.Empty<ChatMessage>();
                }

                // Get only messages for this specific request where user is either sender or receiver
                var spec = new ChatMessagesByRequestIdAndUserSpecification(requestId, userId);
                var messages = await _unitOfWork.Repository<ChatMessage>().GetAllWithSpec(spec);

                return messages;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMessagesForRequestAsync: {ex.Message}");
                return Array.Empty<ChatMessage>();
            }
        }

        public async Task MarkMessagesAsReadAsync(int requestId, int userId)
        {
            try
            {
                // Only mark messages where the current user is the receiver and for this specific request
                var spec = new UnreadMessagesSpecification(userId, requestId);
                var unreadMessages = await _unitOfWork.Repository<ChatMessage>().GetAllWithSpec(spec);

                if (unreadMessages == null || !unreadMessages.Any())
                {
                    return; // No unread messages
                }

                foreach (var message in unreadMessages)
                {
                    message.IsRead = true;
                    _unitOfWork.Repository<ChatMessage>().Update(message);
                }

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MarkMessagesAsReadAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetUnreadMessagesCountAsync(int userId)
        {
            try
            {
                var spec = new BaseSpecifications<ChatMessage>(m => m.ReceiverId == userId && !m.IsRead);
                return await _unitOfWork.Repository<ChatMessage>().GetCount(spec);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUnreadMessagesCountAsync: {ex.Message}");
                return 0;
            }
        }

        public async Task<AppUser> GetUserAsync(int userId)
        {
            try
            {
                return await _userManager.FindByIdAsync(userId.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CanUserAccessChatAsync(int requestId, int appUserId)
        {
            try
            {
                // Get the request with all needed navigation properties
                var spec = new RequestWithDetailsSpecification(requestId);
                var request = await _unitOfWork.Repository<Request>().GetByIdWithSpec(spec);

                // Modified to allow both Pending and Accepted requests
                if (request == null || (request.Status != RequestStatus.Accepted && request.Status != RequestStatus.Pending))
                {
                    Console.WriteLine($"Request {requestId} not found or not in valid status (should be Pending or Accepted)");
                    return false;
                }

                // Verify the user is either the worker or customer for this request
                if (request.Worker?.AppUserId == appUserId)
                {
                    return true; // User is the worker for this request
                }

                if (request.Customer?.AppUserId == appUserId)
                {
                    return true; // User is the customer for this request
                }

                Console.WriteLine($"User {appUserId} has no access to request {requestId}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking access: {ex.Message}");
                return false;
            }
        }
    }
}