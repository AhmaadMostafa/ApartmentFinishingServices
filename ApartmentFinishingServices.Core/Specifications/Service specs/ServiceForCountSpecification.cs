using ApartmentFinishingServices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Service_specs
{
    public class ServiceForCountSpecification : BaseSpecifications<Entities.Services>
    {
        public ServiceForCountSpecification(ServiceSpecParams specParams)
            : base(p =>
                  (string.IsNullOrEmpty(specParams.Search) || p.Name.Contains(specParams.Search))
            )
        { }
    }
}
