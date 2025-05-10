using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Entities.Identity
{
    public class Admin : BaseEntity
    {
        public decimal? TotalEarnings { get; set; } = 0;
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
