using ApartmentFinishingServices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Services.Contract
{
    public interface IReviewService
    {
        Task<Review> AddReviewAsync(int workerId, int appUserId, int rating, string? comment);
        Task<IReadOnlyList<Review>> GetWorkerReviewsAsync(int workerId);
        Task<IReadOnlyList<Review>> GetCustomerReviewsAsync(int appUserId);
        Task<double> GetWorkerAverageRatingAsync(int workerId);
    }
}
