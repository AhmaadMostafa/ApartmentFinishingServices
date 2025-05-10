using ApartmentFinishingServices.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Worker_Specs
{
    public class WorkerWithDetailsSpecification : BaseSpecifications<Worker>
    {
        public WorkerWithDetailsSpecification(WorkerSpecParams specParams)
            :base(w => string.IsNullOrEmpty(specParams.Search) || w.AppUser.Name.Contains(specParams.Search)
             && (!specParams.CityId.HasValue || w.AppUser.CityId == specParams.CityId) &&
                (!specParams.ServiceId.HasValue || w.ServiceId == specParams.ServiceId.Value)
            )
        {
            AddIncludes();
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);

        }
        public WorkerWithDetailsSpecification(int id) : base(p => p.Id == id)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            AddInclude(q => q.Include(p => p.Service).Include(p => p.AppUser).ThenInclude(p => p.City));
        }
    }
}
