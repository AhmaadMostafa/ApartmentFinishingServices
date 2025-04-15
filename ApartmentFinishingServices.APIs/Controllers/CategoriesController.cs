using ApartmentFinishingServices.APIs.Dtos;
using ApartmentFinishingServices.APIs.Helpers;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Repository.Contract;
using ApartmentFinishingServices.Core.Specifications.Category_Specs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentFinishingServices.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IGenricRepository<Category> _categoryRepo;
        private readonly IMapper _mapper;

        public CategoriesController(IGenricRepository<Category> categoryRepo , IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<Pagination<CategoryToReturnDto>>> GetCategory([FromQuery] CategorySpecParams specParams)
        {
            var spec = new CategoryWithIncludesSpecifications(specParams);
            var categories = await _categoryRepo.GetAllWithSpec(spec);
            var data = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryToReturnDto>>(categories);
            var countSpec = new CategoryForCountSpecification(specParams);
            var count = await _categoryRepo.GetCount(countSpec);
            return Ok(new Pagination<CategoryToReturnDto>(specParams.PageIndex, specParams.PageSize, count, data));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryToReturnDto>> GetProduct(int id)
        {
            var spec = new CategoryWithIncludesSpecifications(id);
            var category = await _categoryRepo.GetByIdWithSpec(spec);
            if (category == null)
                return NotFound();
            return Ok(_mapper.Map<Category, CategoryToReturnDto>(category));
        }


    }
}
