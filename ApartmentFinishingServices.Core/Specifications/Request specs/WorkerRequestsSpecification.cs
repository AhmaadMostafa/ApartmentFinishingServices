using ApartmentFinishingServices.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Request_specs
{
    public class WorkerRequestsSpecification : BaseSpecifications<Request>
    {
        public WorkerRequestsSpecification(int workerId)
               : base(r => r.WorkerId == workerId)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            AddInclude(q => q.Include(p => p.Worker).ThenInclude(p => p.AppUser).Include(p => p.Customer)
            .ThenInclude(p => p.AppUser).ThenInclude(p => p.City).Include(p => p.Service));
        }
    }
}
