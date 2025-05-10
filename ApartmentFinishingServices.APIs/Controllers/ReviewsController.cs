using ApartmentFinishingServices.APIs.Dtos;
using ApartmentFinishingServices.APIs.Errors;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Services.Contract;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApartmentFinishingServices.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewsController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ReviewToReturnDto>> AddReview([FromBody] AddReviewDto model)
        {
            var appUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var review = await _reviewService.AddReviewAsync(
                model.WorkerId,
                appUserId,
                model.Rating,
                model.Comment
            );

            if (review is null)
                return BadRequest(new ApiResponse(400, "Unable to add review. Ensure you have completed a service with this worker."));
            if (review.Worker == null || review.Customer == null)
            {
                return StatusCode(500, new ApiResponse(500, "Error loading review details. Please try again."));
            }
            return Ok(_mapper.Map<Review, ReviewToReturnDto>(review));
        }

        [HttpGet("worker/{workerId}")]
        public async Task<ActionResult<IReadOnlyList<ReviewToReturnDto>>> GetWorkerReviews(int workerId)
        {
            var reviews = await _reviewService.GetWorkerReviewsAsync(workerId);
            return Ok(_mapper.Map<IReadOnlyList<Review>, IReadOnlyList<ReviewToReturnDto>>(reviews));
        }

        [HttpGet("my-reviews")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IReadOnlyList<ReviewToReturnDto>>> GetMyReviews()
        {
            var appUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var reviews = await _reviewService.GetCustomerReviewsAsync(appUserId);
            return Ok(_mapper.Map<IReadOnlyList<Review>, IReadOnlyList<ReviewToReturnDto>>(reviews));
        }

        [HttpGet("worker/{workerId}/rating")]
        public async Task<ActionResult<double>> GetWorkerRating(int workerId)
        {
            var rating = await _reviewService.GetWorkerAverageRatingAsync(workerId);
            return Ok(rating);
        }
    }
}
