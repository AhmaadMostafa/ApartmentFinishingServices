using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Specifications.Request_specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Admin_Specs
{
    public class RequestForCountSpecification : BaseSpecifications<Request>
    {
        public RequestForCountSpecification(RequestSpecParams specParams)
            : base(r =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                r.Worker.AppUser.Name.Contains(specParams.Search) || r.Customer.AppUser.Name.Contains(specParams.Search) &&
                (!specParams.Status.HasValue || (int)r.Status == specParams.Status.Value) &&
                (!specParams.WorkerId.HasValue || r.WorkerId == specParams.WorkerId.Value) &&
                (!specParams.CustomerId.HasValue || r.CustomerId == specParams.CustomerId.Value) &&
                (!specParams.ServiceId.HasValue || r.ServiceId == specParams.ServiceId.Value))
            )
        {

        }
    }
}
