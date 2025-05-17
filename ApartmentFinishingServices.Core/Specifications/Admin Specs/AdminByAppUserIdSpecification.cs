using ApartmentFinishingServices.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Admin_Specs
{
    public class AdminByAppUserIdSpecification : BaseSpecifications<Admin>
    {
        public AdminByAppUserIdSpecification(int userId) : base(p => p.AppUserId == userId) { }
    }
}
