using ApartmentFinishingServices.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Worker_Specs
{
    public class WorkerForCountSpecification : BaseSpecifications<Worker>
    {
        public WorkerForCountSpecification(WorkerSpecParams specParams)
        : base(w => string.IsNullOrEmpty(specParams.Search) || w.AppUser.Name.Contains(specParams.Search)
             && (!specParams.CityId.HasValue || w.AppUser.CityId == specParams.CityId) &&
                (!specParams.ServiceId.HasValue || w.ServiceId == specParams.ServiceId.Value))
        { }
    }
}
