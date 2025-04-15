using ApartmentFinishingServices.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Entities
{
    public class City : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<AppUser>? AppUsers { get; set; } = new HashSet<AppUser>();
    }
}
