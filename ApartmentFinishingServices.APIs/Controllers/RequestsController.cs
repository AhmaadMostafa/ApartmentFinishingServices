using ApartmentFinishingServices.APIs.Dtos;
using ApartmentFinishingServices.APIs.Errors;
using ApartmentFinishingServices.Core;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Entities.Identity;
using ApartmentFinishingServices.Core.Repository.Contract;
using ApartmentFinishingServices.Core.Services.Contract;
using ApartmentFinishingServices.Core.Specifications.Request_specs;
using ApartmentFinishingServices.Repository;
using ApartmentFinishingServices.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApartmentFinishingServices.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IMapper _mapper;

        public RequestsController(IRequestService requestService, IMapper mapper)
        {
            _requestService = requestService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<RequestToReturnDto>> CreateRequest([FromBody] CreateRequestDto model)
        {
            var appUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var request = await _requestService.CreateRequestAsync(model.WorkerId, model.ServiceId,
                model.Comment, model.CustomerSuggestedPrice, appUserId);
            if (request is null)
                return BadRequest(new ApiResponse(400));

            return Ok(_mapper.Map<Request, RequestToReturnDto>(request));
        }

        [HttpPut("accept/{id}")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult<RequestToReturnDto>> AcceptRequest(int id)
        {
            var appUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _requestService.AcceptRequestAsync(id, appUserId);
            if (request is null)
                return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Request, RequestToReturnDto>(request));
        }
        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult<RequestToReturnDto>> RejectRequest(int id)
        {
            var appUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _requestService.RejectRequestAsync(id, appUserId);
            if (request is null)
                return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Request, RequestToReturnDto>(request));
        }
        [HttpPut("counter-offer/{id}")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult<RequestToReturnDto>> CounterOffer(int id, [FromBody] decimal price)
        {
            var appUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _requestService.CounterOfferAsync(id, appUserId, price);
            if (request is null)
                return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Request, RequestToReturnDto>(request));
        }

        [HttpPut("respond/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<RequestToReturnDto>> CustomerRespond(int id, [FromBody] CustomerResponseDto response)
        {
            var appUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _requestService.CustomerRespondToOfferAsync(
                id,
                appUserId,
                response.Accept,
                response.NewOffer);
            if (request is null)
                return BadRequest(new ApiResponse(400));

            return Ok(_mapper.Map<Request, RequestToReturnDto>(request));
        }

        [HttpPut("mark-completed/{id}")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult<RequestToReturnDto>> MarkAsCompleted(int id)
        {
            var appUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _requestService.MarkServiceAsCompletedAsync(id, appUserId);

            if (request is null)
                return BadRequest(new ApiResponse(400, "Request not found or cannot be marked as completed"));

            return Ok(_mapper.Map<Request, RequestToReturnDto>(request));
        }


        [HttpGet("my-requests")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IReadOnlyList<RequestToReturnDto>>> GetMyRequests()
        {
            var appUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var requests = await _requestService.GetCustomerRequestsAsync(appUserId);
            return Ok(_mapper.Map<IReadOnlyList<Request>, IReadOnlyList<RequestToReturnDto>>(requests));
        }
        [HttpGet("received-requests")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult<IReadOnlyList<RequestToReturnDto>>> GetReceivedRequests()
        {
            var appUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var requests = await _requestService.GetWorkerRequestsAsync(appUserId);
            return Ok(_mapper.Map<IReadOnlyList<Request>,IReadOnlyList<RequestToReturnDto>>(requests));
        }


    }
}
