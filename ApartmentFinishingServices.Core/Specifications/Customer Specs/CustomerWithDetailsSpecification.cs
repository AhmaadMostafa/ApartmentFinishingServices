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
    public class CustomerWithDetailsSpecification : BaseSpecifications<Customer>
    {
        public CustomerWithDetailsSpecification(CustomerSpecParams specParams)
        : base(w => string.IsNullOrEmpty(specParams.Search) || w.AppUser.Name.Contains(specParams.Search)
        && (!specParams.CityId.HasValue || w.AppUser.CityId == specParams.CityId))
        {
            AddIncludes();
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);


        }
        public CustomerWithDetailsSpecification(int id) : base(p => p.Id == id)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            AddInclude(q => q.Include(p => p.AppUser).ThenInclude(p => p.City));
        }
    }
}
