using ApartmentFinishingServices.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Entities
{
    public class SavedWorker : BaseEntity
    {
        public int CustomerId { get; set; }
        public int ProviderId { get; set; }
        public Customer? Customer { get; set; }
        public Worker? Worker { get; set; }
    }
}
