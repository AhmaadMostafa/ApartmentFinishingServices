using ApartmentFinishingServices.Core;
using ApartmentFinishingServices.Core.Entities.Identity;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Services.Contract;
using ApartmentFinishingServices.Core.Specifications.Request_specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApartmentFinishingServices.Core.Specifications.Review_Specs;

namespace ApartmentFinishingServices.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Review> AddReviewAsync(int workerId, int appUserId, int rating, string? comment = null)
        {
            var customerSpec = new CustomerByAppUserIdSpecification(appUserId);
            var customer = await _unitOfWork.Repository<Customer>().GetByIdWithSpec(customerSpec);

            var completedRequestsSpec = new CompletedRequestsByCustomerAndWorkerSpecification(customer.Id, workerId);
            var hasCompletedRequests = await _unitOfWork.Repository<Request>().GetCount(completedRequestsSpec);

            if (hasCompletedRequests == 0)
                return null;

            var review = new Review
            {
                WorkerId = workerId,
                CustomerId = customer.Id,
                Comment = comment,
                Rating = rating
            };


            await _unitOfWork.Repository<Review>().Add(review);
            await _unitOfWork.CompleteAsync();

            var reviewSpec = new ReviewWithDetailsSpecification(review.Id);

            return await _unitOfWork.Repository<Review>().GetByIdWithSpec(reviewSpec);
        }

        public async Task<IReadOnlyList<Review>> GetCustomerReviewsAsync(int appUserId)
        {
            var customerSpec = new CustomerByAppUserIdSpecification(appUserId);
            var customer = await _unitOfWork.Repository<Customer>().GetByIdWithSpec(customerSpec);

            var spec = new CustomerReviewsSpecification(customer.Id);
            return await _unitOfWork.Repository<Review>().GetAllWithSpec(spec);
        }


        public async Task<IReadOnlyList<Review>> GetWorkerReviewsAsync(int workerId)
        {
            var spec = new WorkerReviewsSpecification(workerId);
            return await _unitOfWork.Repository<Review>().GetAllWithSpec(spec);
        }

        public async Task<double> GetWorkerAverageRatingAsync(int workerId)
        {
            var spec = new WorkerReviewsSpecification(workerId);
            var reviews = await _unitOfWork.Repository<Review>().GetAllWithSpec(spec);

            if (reviews is null || !reviews.Any())
                return 0;

            return reviews.Average(r => r.Rating);
        }
    }
}
