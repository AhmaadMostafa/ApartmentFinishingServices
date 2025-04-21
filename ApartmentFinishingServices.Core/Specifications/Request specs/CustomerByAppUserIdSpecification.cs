using ApartmentFinishingServices.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Request_specs
{
    public class CustomerByAppUserIdSpecification : BaseSpecifications<Customer>
    {
        public CustomerByAppUserIdSpecification(int appUserId) : base(c => c.AppUserId == appUserId)
        {
        }
    }
}
