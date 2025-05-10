using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Entities.Identity
{
    public class Worker : BaseEntity
    {
        public string? Description { get; set; }
        public int Rating { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? ServiceId { get; set; }
        public decimal? TotalEarnings { get; set; }
        public Services? Service { get; set; }
        public ICollection<PortfolioItem>? PortfolioItems { get; set; } = new HashSet<PortfolioItem>();
        public ICollection<AvailableDay>? AvailableDays { get; set; } = new HashSet<AvailableDay>();
        public ICollection<Review>? Reviews { get; set; } = new HashSet<Review>();
        public ICollection<SavedWorker>? SavedWorkers { get; set; } = new HashSet<SavedWorker>();
        public int? CompletedRequests { get; set; } = 0;
        //public ICollection<Request>? Requests { get; set; } = new HashSet<Request>();
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
