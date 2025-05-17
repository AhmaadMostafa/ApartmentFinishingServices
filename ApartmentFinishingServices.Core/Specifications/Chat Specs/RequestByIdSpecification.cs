using ApartmentFinishingServices.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Chat_Specs
{
    public class RequestByIdSpecification : BaseSpecifications<Request>
    {
        public RequestByIdSpecification(int requestId)
            : base(r => r.Id == requestId)
        {
            AddInclude(r => r.Include(p => p.Customer).Include(p => p.Worker));
        }

    }
}
