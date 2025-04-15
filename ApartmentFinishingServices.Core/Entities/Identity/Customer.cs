using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Entities.Identity
{
    public class Customer : BaseEntity
    {
        public ICollection<Request>? Requests { get; set; } = new HashSet<Request>();
        public ICollection<Review>? Reviews { get; set; } = new HashSet<Review>();
        public ICollection<SavedWorker>? SavedWorkers { get; set; } = new HashSet<SavedWorker>();
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
