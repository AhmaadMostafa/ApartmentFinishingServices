using ApartmentFinishingServices.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Chat_Specs
{
    public class UnreadMessagesSpecification : BaseSpecifications<ChatMessage>
    {
        public UnreadMessagesSpecification(int receiverId , int requestId)
            :base(m => m.ReceiverId == receiverId && m.RequestId == requestId && !m.IsRead)
        {
            AddInclude(p => p.Include(m => m.Sender));
        }
    }
}
