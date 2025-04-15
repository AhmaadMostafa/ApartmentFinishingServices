using ApartmentFinishingServices.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Entities
{
    public class Services : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<Worker>? Workers { get; set; } = new HashSet<Worker>();
        public ICollection<Request>? Requests { get; set; } = new HashSet<Request>();
    }
}
