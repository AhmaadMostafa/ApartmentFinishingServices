using ApartmentFinishingServices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Category_Specs
{
    public class CategoryForCountSpecification : BaseSpecifications<Category>
    {
        public CategoryForCountSpecification(CategorySpecParams specParams)
            : base(p =>
                  (string.IsNullOrEmpty(specParams.Search) || p.Name.Contains(specParams.Search))        
            )
        {

        }
    }
}
