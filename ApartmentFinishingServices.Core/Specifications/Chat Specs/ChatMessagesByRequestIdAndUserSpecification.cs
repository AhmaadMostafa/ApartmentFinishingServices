using ApartmentFinishingServices.Core;
using ApartmentFinishingServices.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace ApartmentFinishingServices.Core.Specifications.Chat_Specs
{
    public class ChatMessagesByRequestIdAndUserSpecification : BaseSpecifications<ChatMessage>
    {
        public ChatMessagesByRequestIdAndUserSpecification(int requestId, int userId)
            : base(m =>
                  m.RequestId == requestId &&
                  (m.SenderId == userId || m.ReceiverId == userId))
        {
            // Include related entities
            AddInclude(m => m.Include(p => p.Sender).Include(p => p.Receiver));

            // Order by sent time
            AddOrderBy(m => m.SentAt);
        }
    }
}