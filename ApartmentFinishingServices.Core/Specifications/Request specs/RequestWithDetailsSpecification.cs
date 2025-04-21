using ApartmentFinishingServices.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Request_specs
{
    public class RequestWithDetailsSpecification : BaseSpecifications<Request>
    {
        public RequestWithDetailsSpecification(int id) : base(r => r.Id == id)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            AddInclude(q => q.Include(p => p.Worker).Include(p => p.Worker.AppUser).Include(p => p.Customer)
            .Include(p => p.Customer.AppUser).Include(p => p.Service));
        }
    }
}
