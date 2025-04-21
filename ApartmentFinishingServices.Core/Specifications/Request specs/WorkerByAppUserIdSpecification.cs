using ApartmentFinishingServices.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Request_specs
{
    public class WorkerByAppUserIdSpecification : BaseSpecifications<Worker>
    {
        public WorkerByAppUserIdSpecification(int userId)
            : base(c => c.AppUserId == userId)
        {
        }
    }
}
