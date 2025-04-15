using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Specifications.Category_Specs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Service_specs
{
    public class ServiceWithIncludesSpecifications : BaseSpecifications<Entities.Services>
    {
        public ServiceWithIncludesSpecifications(ServiceSpecParams specParams)
            : base(p =>
                  string.IsNullOrEmpty(specParams.Search) || p.Name.Contains(specParams.Search))
        {
            AddIncludes();
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                AddOrderBy(p => p.Name);
            }

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }
        public ServiceWithIncludesSpecifications(int id) : base(p => p.Id == id)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            AddInclude(q => q.Include(p => p.Workers).ThenInclude(p => p.AppUser).ThenInclude(u => u.City));
        }
    }
}
