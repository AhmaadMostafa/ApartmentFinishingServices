using ApartmentFinishingServices.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Category_Specs
{
    public class CategoryWithIncludesSpecifications : BaseSpecifications<Category>
    {
        public CategoryWithIncludesSpecifications(CategorySpecParams specParams)
            : base(c =>
                 (string.IsNullOrEmpty(specParams.Search) || c.Name.Contains(specParams.Search))
            )
        {
            AddIncludes();
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                AddOrderBy(p => p.Name);
            }

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);

        }
        public CategoryWithIncludesSpecifications(int id) : base(p => p.Id == id)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            AddInclude(q => q.Include(p => p.Services).ThenInclude(p => p.Workers).ThenInclude(u => u.AppUser).ThenInclude(c => c.City));
            //AddInclude(q => q.Include(p => p.Category));
        }
    }
}
