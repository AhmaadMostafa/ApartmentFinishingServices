using ApartmentFinishingServices.APIs.Dtos;
using ApartmentFinishingServices.APIs.Helpers;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Repository.Contract;
using ApartmentFinishingServices.Core.Specifications.Category_Specs;
using ApartmentFinishingServices.Core.Specifications.Service_specs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentFinishingServices.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IGenricRepository<Services> _serviceRepo;
        private readonly IMapper _mapper;

        public ServicesController(IGenricRepository<Services> serviceRepo, IMapper mapper)
        {
            _serviceRepo = serviceRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<Pagination<ServiceDto>>> GetServices([FromQuery] ServiceSpecParams specParams)
        {
            var spec = new ServiceWithIncludesSpecifications(specParams);
            var services = await _serviceRepo.GetAllWithSpec(spec);
            var data = _mapper.Map<IReadOnlyList<Services>, IReadOnlyList<ServiceDto>>(services);
            var countSpec = new ServiceForCountSpecification(specParams);
            var count = await _serviceRepo.GetCount(countSpec);
            return Ok(new Pagination<ServiceDto>(specParams.PageIndex, specParams.PageSize, count, data));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDto>> GetService(int id)
        {
            var spec = new ServiceWithIncludesSpecifications(id);
            var service = await _serviceRepo.GetByIdWithSpec(spec);
            if (service == null)
                return NotFound();
            return Ok(_mapper.Map<Services, ServiceDto>(service));
        }

    }
}
