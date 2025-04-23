using ApartmentFinishingServices.APIs.Dtos;
using ApartmentFinishingServices.APIs.Errors;
using ApartmentFinishingServices.APIs.Helpers;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Repository.Contract;
using ApartmentFinishingServices.Core.Services.Contract;
using ApartmentFinishingServices.Core.Specifications.Category_Specs;
using ApartmentFinishingServices.Core.Specifications.Service_specs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApartmentFinishingServices.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        [HttpGet]
        //[Authorize(Roles = "Customer")]
        public async Task<ActionResult<Pagination<CategoryToReturnDto>>> GetCategories([FromQuery] CategorySpecParams specParams)
        {
            var categories = await _categoryService.GetCategoriesAsync(specParams);
            var data = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryToReturnDto>>(categories);
            var count = await _categoryService.GetCategoryCountAsync(specParams);
            return Ok(new Pagination<CategoryToReturnDto>(specParams.PageIndex, specParams.PageSize, count, data));
        }
        [HttpGet("{id}")]
        //[Authorize(Roles = "Customer")]

        public async Task<ActionResult<CategoryToReturnDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryAsync(id);
            if (category == null)
                return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<Category, CategoryToReturnDto>(category));
        }
        [HttpGet("services")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Pagination<ServiceDto>>> GetServices([FromQuery] ServiceSpecParams specParams)
        {
            var services = await _categoryService.GetServicesAsync(specParams);
            var data = _mapper.Map<IReadOnlyList<Services>, IReadOnlyList<ServiceDto>>(services);
            var count = await _categoryService.GetServiceCountAsync(specParams);
            return Ok(new Pagination<ServiceDto>(specParams.PageIndex, specParams.PageSize, count, data));
        }
        [HttpGet("services/{id}")]
        public async Task<ActionResult<ServiceDto>> GetService(int id)
        {
            var service = await _categoryService.GetServiceAsync(id);
            if (service == null)
                return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<Services, ServiceDto>(service));
        }


    }
}
