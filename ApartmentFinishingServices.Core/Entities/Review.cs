using ApartmentFinishingServices.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Entities
{
    public class Review : BaseEntity
    {
        public int WorkerId { get; set; }
        public int CustomerId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public Worker? Worker { get; set; }
        public Customer? Customer { get; set; }
    }
}
