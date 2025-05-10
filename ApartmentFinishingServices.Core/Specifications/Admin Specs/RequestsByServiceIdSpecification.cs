using ApartmentFinishingServices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Admin_Specs
{
    public class RequestsByServiceIdSpecification : BaseSpecifications<Request>
    {
        public RequestsByServiceIdSpecification(int serviceId)
            : base(r => r.ServiceId == serviceId)
        {

        }

    }
}
