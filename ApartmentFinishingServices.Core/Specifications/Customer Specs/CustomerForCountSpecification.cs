using ApartmentFinishingServices.Core.Entities.Identity;
using ApartmentFinishingServices.Core.Specifications.Worker_Specs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Customer_Specs
{
    public class CustomerForCountSpecification : BaseSpecifications<Customer>
    {
        public CustomerForCountSpecification(CustomerSpecParams specParams)
            : base(w => string.IsNullOrEmpty(specParams.Search) || w.AppUser.Name.Contains(specParams.Search)
            && (!specParams.CityId.HasValue || w.AppUser.CityId == specParams.CityId))
        {
        }

    }
}
