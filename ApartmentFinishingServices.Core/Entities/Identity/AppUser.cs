using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Entities.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
