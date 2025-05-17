using ApartmentFinishingServices.APIs.Dtos.ChatDtos;
using ApartmentFinishingServices.APIs.Errors;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Services.Contract;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApartmentFinishingServices.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;

        public ChatController(IChatService chatService, IMapper mapper)
        {
            _chatService = chatService;
            _mapper = mapper;
        }

        [HttpGet("request/{requestId}")]
        public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetMessages(int requestId)
        {
            try
            {
                // Get user ID from claims
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var appUserId))
                {
                    return Unauthorized(new ApiResponse(401, "User not authenticated"));
                }

                // Check if user has access to this chat
                var canAccess = await _chatService.CanUserAccessChatAsync(requestId, appUserId);
                if (!canAccess)
                {
                    return Forbid();
                }

                // Get request participants to ensure we're only dealing with the two users
                var participants = await _chatService.GetRequestParticipantsAsync(requestId);
                if (participants == null)
                {
                    return NotFound(new ApiResponse(404, "Request not found or not accepted"));
                }

                // Verify the current user is one of the participants
                if (participants.WorkerId != appUserId && participants.CustomerId != appUserId)
                {
                    return Forbid();
                }

                // Get messages between these two specific users only
                var messages = await _chatService.GetMessagesForRequestAsync(requestId, appUserId);

                // Mark messages as read since user is viewing them
                await _chatService.MarkMessagesAsReadAsync(requestId, appUserId);

                // Map to DTOs and include sender/receiver names
                var result = _mapper.Map<IReadOnlyList<ChatMessage>, IReadOnlyList<MessageDto>>(messages);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetMessages: {ex.Message}");
                return StatusCode(500, new ApiResponse(500, $"An error occurred: {ex.Message}"));
            }
        }

        [HttpGet("participants/{requestId}")]
        public async Task<ActionResult<RequestParticipantsDto>> GetChatParticipants(int requestId)
        {
            try
            {
                // Get user ID from claims
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var appUserId))
                {
                    return Unauthorized(new ApiResponse(401, "User not authenticated"));
                }

                // Check if user has access to this chat
                var canAccess = await _chatService.CanUserAccessChatAsync(requestId, appUserId);
                if (!canAccess)
                {
                    return Forbid();
                }

                var participants = await _chatService.GetRequestParticipantsAsync(requestId);
                if (participants == null)
                {
                    return NotFound(new ApiResponse(404, "Request not found or not accepted"));
                }

                return Ok(participants);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetChatParticipants: {ex.Message}");
                return StatusCode(500, new ApiResponse(500, $"An error occurred: {ex.Message}"));
            }
        }

        [HttpPost("mark-read/{requestId}")]
        public async Task<IActionResult> MarkMessagesAsRead(int requestId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Unauthorized(new ApiResponse(401, "User not authenticated"));
            }

            // Check if user has access to this chat
            var canAccess = await _chatService.CanUserAccessChatAsync(requestId, userId);
            if (!canAccess)
            {
                return Forbid();
            }

            await _chatService.MarkMessagesAsReadAsync(requestId, userId);
            return Ok();
        }

        [HttpGet("unread-count")]
        public async Task<ActionResult<int>> GetUnreadMessagesCount()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Unauthorized(new ApiResponse(401, "User not authenticated"));
            }

            var count = await _chatService.GetUnreadMessagesCountAsync(userId);
            return Ok(count);
        }
    }
}