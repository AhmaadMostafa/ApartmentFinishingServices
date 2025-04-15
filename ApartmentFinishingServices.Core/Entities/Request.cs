using ApartmentFinishingServices.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Entities
{
    public enum PriceNegotiationStatus
    {
        Pending,       
        CustomerOffer,  
        WorkerOffer,    
        Accepted,       
        Rejected  
    }
    public class Request : BaseEntity
    {
        public int WorkerId { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public string? Comment { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public decimal? CustomerSuggestedPrice { get; set; }
        public decimal? WorkerSuggestedPrice { get; set; } 
        public decimal? FinalAgreedPrice { get; set; }
        public PriceNegotiationStatus NegotiationStatus { get; set; } = PriceNegotiationStatus.Pending;
        public Customer Customer { get; set; }
        public Worker Worker { get; set; }
        public Services Service { get; set; }
    }
}
