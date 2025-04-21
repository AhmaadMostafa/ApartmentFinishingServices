using ApartmentFinishingServices.Core;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Entities.Identity;
using ApartmentFinishingServices.Core.Services.Contract;
using ApartmentFinishingServices.Core.Specifications.Request_specs;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Service
{
    public class RequestService : IRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Request> CreateRequestAsync(int workerId , int serviceId , string? comment , decimal? customerSuggestedPrice , int appUserId)
        {
            var customerSpec = new CustomerByAppUserIdSpecification(appUserId);

            var customer = await _unitOfWork.Repository<Customer>().GetByIdWithSpec(customerSpec);

            var request = new Request
            {
                WorkerId = workerId,
                CustomerId = customer.Id,
                ServiceId = serviceId,
                Comment = comment,
                CustomerSuggestedPrice = customerSuggestedPrice,
                Status = RequestStatus.Pending,
                NegotiationStatus = customerSuggestedPrice.HasValue?
                                                          PriceNegotiationStatus.CustomerOffer : 
                                                          PriceNegotiationStatus.Pending                    
            };

            await _unitOfWork.Repository<Request>().Add(request);
            await _unitOfWork.CompleteAsync();
            var spec = new RequestWithDetailsSpecification(request.Id);
            return await _unitOfWork.Repository<Request>().GetByIdWithSpec(spec);

        }

        public async Task<Request> AcceptRequestAsync(int requestId , int appUserId)
        {
            var workerSpec = new WorkerByAppUserIdSpecification(appUserId);
            var worker = await _unitOfWork.Repository<Worker>().GetByIdWithSpec(workerSpec);
            var spec = new RequestByIdAndWorkerIdSpecification(requestId, worker.Id);
            var request = await _unitOfWork.Repository<Request>().GetByIdWithSpec(spec);
            request.Status = RequestStatus.Accepted;
            request.NegotiationStatus = PriceNegotiationStatus.Accepted;
            request.FinalAgreedPrice = request.WorkerSuggestedPrice ?? request.CustomerSuggestedPrice;
            _unitOfWork.Repository<Request>().Update(request);
            await _unitOfWork.CompleteAsync();
            return request;
        }

        public async Task<Request> RejectRequestAsync(int requestId , int appUserId)
        {
            var workerSpec = new WorkerByAppUserIdSpecification(appUserId);
            var worker = await _unitOfWork.Repository<Worker>().GetByIdWithSpec(workerSpec);
            var spec = new RequestByIdAndWorkerIdSpecification(requestId, worker.Id);
            var request = await _unitOfWork.Repository<Request>().GetByIdWithSpec(spec);
            request.Status = RequestStatus.Rejected;
            request.NegotiationStatus = PriceNegotiationStatus.Rejected;
            _unitOfWork.Repository<Request>().Update(request);
            await _unitOfWork.CompleteAsync();
            return request;
        }
        public async Task<Request> CounterOfferAsync(int requestId, int appUserId, decimal price)
        {
            var workerSpec = new WorkerByAppUserIdSpecification(appUserId);
            var worker = await _unitOfWork.Repository<Worker>().GetByIdWithSpec(workerSpec);
            var spec = new RequestByIdAndWorkerIdSpecification(requestId, worker.Id);
            var request = await _unitOfWork.Repository<Request>().GetByIdWithSpec(spec);
            request.WorkerSuggestedPrice = price;
            request.Status = RequestStatus.Pending;
            request.NegotiationStatus = PriceNegotiationStatus.WorkerOffer;

            _unitOfWork.Repository<Request>().Update(request);
            await _unitOfWork.CompleteAsync();
            return request;
        }
        public async Task<Request> CustomerRespondToOfferAsync(int requestId, int appUserId, bool accept, decimal? newOffer = null)
        {
            var customerSpec = new CustomerByAppUserIdSpecification(appUserId);
            var customer = await _unitOfWork.Repository<Customer>().GetByIdWithSpec(customerSpec);

            var spec = new RequestByIdAndCustomerIdSpecification(requestId, customer.Id);
            var request = await _unitOfWork.Repository<Request>().GetByIdWithSpec(spec);

            if (accept && newOffer is null)
            {
                request.Status = RequestStatus.Accepted;
                request.NegotiationStatus = PriceNegotiationStatus.Accepted;
                request.FinalAgreedPrice = request.WorkerSuggestedPrice;
            }
            else if (newOffer.HasValue)
            {
                request.Status = RequestStatus.Pending;
                request.CustomerSuggestedPrice = newOffer;
                request.NegotiationStatus = PriceNegotiationStatus.CustomerOffer;
            }
            else
            {
                request.Status = RequestStatus.Rejected;
                request.NegotiationStatus = PriceNegotiationStatus.Rejected;
            }

            _unitOfWork.Repository<Request>().Update(request);
            await _unitOfWork.CompleteAsync();
            return request;
        }
        public async Task<IReadOnlyList<Request>> GetCustomerRequestsAsync(int appUserId)
        {
            var customerSpec = new CustomerByAppUserIdSpecification(appUserId);
            var customer = await _unitOfWork.Repository<Customer>().GetByIdWithSpec(customerSpec);
            var spec = new CustomerRequestsSpecification(customer.Id);
            var requests = await _unitOfWork.Repository<Request>().GetAllWithSpec(spec);
            return requests;
        }
        public async Task<IReadOnlyList<Request>> GetWorkerRequestsAsync(int appUserId)
        {

            var workerSpec = new WorkerByAppUserIdSpecification(appUserId);
            var worker = await _unitOfWork.Repository<Worker>().GetByIdWithSpec(workerSpec);
            var spec = new WorkerRequestsSpecification(worker.Id);
            var requests = await _unitOfWork.Repository<Request>().GetAllWithSpec(spec);
            return requests;
        }



    }
}
