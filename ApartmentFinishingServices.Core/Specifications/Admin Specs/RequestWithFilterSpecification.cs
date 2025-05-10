using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Specifications.Request_specs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Admin_Specs
{
    public class RequestWithFilterSpecification : BaseSpecifications<Request>
    {
        public RequestWithFilterSpecification(RequestSpecParams specParams)
            : base(r =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                        r.Worker.AppUser.Name.Contains(specParams.Search) ||
                        r.Customer.AppUser.Name.Contains(specParams.Search)) &&
                (!specParams.Status.HasValue || (int)r.Status == specParams.Status.Value) &&
                (!specParams.WorkerId.HasValue || r.WorkerId == specParams.WorkerId.Value) &&
                (!specParams.CustomerId.HasValue || r.CustomerId == specParams.CustomerId.Value) &&
                (!specParams.ServiceId.HasValue || r.ServiceId == specParams.ServiceId.Value))
        {
            AddIncludes();
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);

        }
        private void AddIncludes()
        {
            AddInclude(q => q.Include(p => p.Customer.AppUser).Include(p => p.Worker.AppUser).Include(p => p.Service));
        }
    }
}
