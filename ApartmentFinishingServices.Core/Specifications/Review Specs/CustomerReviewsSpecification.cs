using ApartmentFinishingServices.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Review_Specs
{
    public class CustomerReviewsSpecification : BaseSpecifications<Review>
    {
        public CustomerReviewsSpecification(int customerId) : base(r => r.CustomerId == customerId)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            AddInclude(q => q.Include(p => p.Worker).Include(p => p.Worker.AppUser).Include(p => p.Customer)
            .Include(p => p.Customer.AppUser));
        }
    }
}
