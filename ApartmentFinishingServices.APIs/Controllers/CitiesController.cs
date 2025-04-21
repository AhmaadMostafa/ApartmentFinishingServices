using ApartmentFinishingServices.APIs.Dtos;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Repository.Contract;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentFinishingServices.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IGenricRepository<City> _cityRepo;
        private readonly IMapper _mapper;

        public CitiesController(IGenricRepository<City> cityRepo , IMapper mapper)
        {
            _cityRepo = cityRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CityDto>> GetCities()
        {
            var cities = await _cityRepo.GetAll();
            return Ok(_mapper.Map<IReadOnlyList<City>, List<CityDto>>(cities));
        }
    }
}
