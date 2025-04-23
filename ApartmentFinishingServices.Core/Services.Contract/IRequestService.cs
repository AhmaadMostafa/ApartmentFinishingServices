using ApartmentFinishingServices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Services.Contract
{
    public interface IRequestService
    {
        Task<Request> CreateRequestAsync(int workerId, int serviceId, string? comment, decimal? customerSuggestedPrice, int customerId);
        Task<Request> AcceptRequestAsync(int requestId, int workerId);
        Task<Request> RejectRequestAsync(int requestId, int workerId);
        Task<Request> CounterOfferAsync(int requestId, int workerId, decimal price);
        Task<Request> CustomerRespondToOfferAsync(int requestId, int customerId, bool accept, decimal? newOffer = null);
        Task<Request> MarkServiceAsCompletedAsync(int requestId, int appUserId);
        Task<IReadOnlyList<Request>> GetCustomerRequestsAsync(int customerId);
        Task<IReadOnlyList<Request>> GetWorkerRequestsAsync(int workerId);
    }
}
