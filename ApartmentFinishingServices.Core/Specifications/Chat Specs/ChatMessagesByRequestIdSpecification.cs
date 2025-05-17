using ApartmentFinishingServices.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Chat_Specs
{
    public class ChatMessagesByRequestIdSpecification : BaseSpecifications<ChatMessage>
    {
        public ChatMessagesByRequestIdSpecification(int requestId)
            :base(m => m.RequestId == requestId)
        {
            AddInclude(r => r.Include(p => p.Sender).Include(p => p.Receiver));
            AddOrderByDesc(m => m.SentAt);
        }

    }
}
