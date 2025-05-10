using ApartmentFinishingServices.APIs.Dtos;
using ApartmentFinishingServices.APIs.Dtos.AdminDtos;
using ApartmentFinishingServices.APIs.Errors;
using ApartmentFinishingServices.APIs.Helpers;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Entities.Identity;
using ApartmentFinishingServices.Core.Services.Contract;
using ApartmentFinishingServices.Core.Specifications.Customer_Specs;
using ApartmentFinishingServices.Core.Specifications.Request_specs;
using ApartmentFinishingServices.Core.Specifications.Worker_Specs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentFinishingServices.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IReviewService _reviewService;
        private readonly IRequestService _requestService;
        private readonly IMapper _mapper;

        public AdminController(IAdminService adminService, IReviewService reviewService, IRequestService requestService, IMapper mapper)
        {
            _adminService = adminService;
            _reviewService = reviewService;
            _requestService = requestService;
            _mapper = mapper;
        }
        [HttpGet("workers")]
        public async Task<ActionResult<Pagination<WorkerListDto>>> GetWorkers([FromQuery] WorkerSpecParams specParams)
        {
            var workers = await _adminService.GetAllWorkersAsync(specParams);
            var data = _mapper.Map<IReadOnlyList<Worker>, IReadOnlyList<WorkerListDto>>(workers);
            var count = await _adminService.GetWorkersCountAsync(specParams);
            return Ok(new Pagination<WorkerListDto>(specParams.PageIndex, specParams.PageSize ,count, data));
        }
        [HttpGet("customers")]
        public async Task<ActionResult<Pagination<CustomerListDto>>> GetCustomers([FromQuery] CustomerSpecParams specParams)
        {
            var customers = await _adminService.GetAllCustomersAsync(specParams);
            var count = await _adminService.GetCustomersCountAsync(specParams);
            var data = _mapper.Map<IReadOnlyList<Customer>, IReadOnlyList<CustomerListDto>>(customers);
            return Ok(new Pagination<CustomerListDto>(specParams.PageIndex, specParams.PageSize, count, data));
        }

        [HttpGet("requests")]
        public async Task<ActionResult<Pagination<RequestToReturnDto>>> GetAllRequests([FromQuery] RequestSpecParams specParams)
        {
            var requests = await _adminService.GetAllRequestsAsync(specParams);
            var data = _mapper.Map<IReadOnlyList<Request>, IReadOnlyList<RequestToReturnDto>>(requests);

            var count = await _adminService.GetRequestsCountAsync(specParams);

            return Ok(new Pagination<RequestToReturnDto>(
                specParams.PageIndex,
                specParams.PageSize,
                count,
                data));
        }
        [HttpGet("workers/{id}")]

        public async Task<ActionResult<WorkerListDto>> GetWorker(int id)
        {
            var worker = await _adminService.GetWorkerAsync(id);
            if (worker == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Worker, WorkerListDto>(worker));
        }
        [HttpGet("customers/{id}")]

        public async Task<ActionResult<CustomerListDto>> GetCustomer(int id)
        {
            var customer = await _adminService.GetCustomerAsync(id);
            if (customer == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Customer, CustomerListDto>(customer));
        }
        [HttpGet("requests/{id}")]
        public async Task<ActionResult<RequestToReturnDto>> GetRequest(int id)
        {
            var request =  await _adminService.GetRequestAsync(id);
            if (request == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Request, RequestToReturnDto>(request));

        }
        [HttpPost("categories")]
        public async Task<ActionResult<CategoryToReturnDto>> CreateCategory([FromBody] CreateCategoryDto model)
        {
            var category = await _adminService.AddCategoryAsync(model.Name, model.PictureUrl);
            return Ok(_mapper.Map<Category , CategoryToReturnDto>(category));
        }

        [HttpDelete("categories/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var result = await _adminService.DeleteCategoryAsync(id);

            if (!result)
                return BadRequest(new ApiResponse(400, "Cannot delete this category. It may not exist or may have services associated with it."));

            return Ok();
        }
        [HttpPost("services")]
        public async Task<ActionResult<ServiceDto>> CreateService([FromBody] CreateServiceDto model)
        {
            var service = await _adminService.AddServiceAsync(model.Name, model.CategoryId);
            return Ok(_mapper.Map<Services, ServiceDto>(service));
        }
        [HttpDelete("services/{id}")]
        public async Task<ActionResult> DeleteService(int id)
        {
            var result = await _adminService.DeleteServiceAsync(id);

            if (!result)
                return BadRequest(new ApiResponse(400, "Cannot delete this service. It may not exist or may have requests associated with it."));

            return Ok();
        }

        [HttpPost("block-user/{id}")]
        public async Task<ActionResult> BlockUser(int id)
        {
            var user = await _adminService.LockUserAsync(id);
            if (!user) return BadRequest("Failed to lock user");
            return NoContent();
        }
        [HttpPost("unblock-user/{id}")]
        public async Task<IActionResult> UnblockUser(int id)
        {
            var user = await _adminService.UnlockUserAsync(id);
            if (!user) return BadRequest(new ApiResponse(400 , "Failed to unlock user"));
            return NoContent();
        }
    }
}
