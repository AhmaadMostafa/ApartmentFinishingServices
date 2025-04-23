using ApartmentFinishingServices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Request_specs
{
    public class CompletedRequestsByCustomerAndWorkerSpecification : BaseSpecifications<Request>
    {
        public CompletedRequestsByCustomerAndWorkerSpecification(int customerId, int workerId)
            : base(r => r.CustomerId == customerId &&
                   r.WorkerId == workerId &&
                   r.Status == RequestStatus.Completed)
        {
        }
    }
}
