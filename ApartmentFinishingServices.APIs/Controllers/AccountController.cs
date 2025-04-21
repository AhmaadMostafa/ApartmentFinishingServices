using ApartmentFinishingServices.APIs.Dtos;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Entities.Identity;
using ApartmentFinishingServices.Core.Repository.Contract;
using ApartmentFinishingServices.Core.Services.Contract;
using ApartmentFinishingServices.Service;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentFinishingServices.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IGenricRepository<Customer> _customerRepo;
        private readonly IGenricRepository<Worker> _workerRepo;
        private readonly IGenricRepository<City> _cityRepo;
        private readonly IGenricRepository<AvailableDay> _availableDayRepo;
        private readonly IGenricRepository<Services> _serviceRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IAuthService authService , IGenricRepository<Customer> customerRepo , IGenricRepository<City> cityRepo
            , IGenricRepository<Worker> workerRepo , IGenricRepository<Services> serviceRepo, IMapper mapper
            , IGenricRepository<AvailableDay> availableDayRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _cityRepo = cityRepo;
            _serviceRepo = serviceRepo;
            _availableDayRepo = availableDayRepo;
            _customerRepo = customerRepo;
            _workerRepo = workerRepo;
            _mapper = mapper;
        }
        private async Task<ActionResult<AppUser>> RegisterUser(RegisterBaseDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new { Error = "Email is already taken" });
            if (model.Password != model.ConfirmPassword)
                return BadRequest(new { Error = "Password do not match" });
            var cityExist = await _cityRepo.GetById(model.CityId);
            if (cityExist is null)
                return BadRequest();
            var user = new AppUser()
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                CityId = model.CityId,
                Age = model.Age,
                ProfilePictureUrl = model.ProfilePictureUrl,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded is false)
            {
                return BadRequest(new { Error = "Failed to create user", Errors = result.Errors.Select(e => e.Description) });
            }
            return user;
        }

        [HttpPost("register/customer")]
        public async Task<ActionResult<CustomerToReturnDto>> RegisterAsCustomer(RegisterAsCustomerDto model)
        {
            var registrationResult = await RegisterUser(model);
            var user = registrationResult.Value;
            var customer = new Customer{ AppUserId = user.Id};
            await _customerRepo.Add(customer);
            await _userManager.AddToRoleAsync(user, "Customer");
            return Ok(new CustomerToReturnDto()
            {
                DisplayName = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CityName = user.City.Name,
                Age= user.Age,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }
        [HttpPost("login")]
        public async Task<ActionResult<CustomerToReturnDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized();
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            var cityExist = await _cityRepo.GetById(user.CityId);
            if (result.Succeeded is false)
                return Unauthorized();
            return Ok(new CustomerToReturnDto()
            {
                DisplayName = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CityName = cityExist?.Name,
                Age = user.Age,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }
        [HttpGet("emailexists")] // GET /api/accounts/emailexists?email=ahmedmostafa8011@gmail.com
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
        [HttpPost("register/worker")]
        public async Task<ActionResult<WorkerToReturnDto>>RegisterAsWorker(RegisterAsWorkerDto model)
        {
            var registrationResult = await RegisterUser(model);
            var user = registrationResult.Value;
            var serviceExist = await _serviceRepo.GetById(model.ServiceId);
            var worker = new Worker()
            {
                Description = string.IsNullOrEmpty(model.Description) ? "New Worker" : model.Description,
                Rating = 0,
                MinPrice = model.MinPrice,
                MaxPrice = model.MaxPrice,
                ServiceId = model.ServiceId,
                AppUserId = user.Id
            };
            if (model.AvailableDays != null && model.AvailableDays.Any())
            {
                foreach (var dayInfo in model.AvailableDays)
                {
                    var availableDay = new AvailableDay
                    {
                        WorkerId = worker.Id,
                        Day = dayInfo.Day,
                        StartTime = TimeOnly.Parse(dayInfo.StartTime),
                        EndTime = TimeOnly.Parse(dayInfo.EndTime),
                        Worker = worker
                    };

                    // Assuming you have a repository for AvailableDays
                    await _availableDayRepo.Add(availableDay);
                }
            }
            await _workerRepo.Add(worker);

            await _userManager.AddToRoleAsync(user, "Worker");
            return Ok(new WorkerToReturnDto
            {
                DisplayName = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CityName = user.City.Name,
                Age = user.Age,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Address = user.Address,
                WorkerId = worker.Id,
                ServiceName = serviceExist.Name,
                Description = worker.Description,
                MinPrice = worker.MinPrice,
                MaxPrice = worker.MaxPrice,
                AvailableDays = worker.AvailableDays?.Select(d => new AvailableDayResponseDto
                {
                    Day = d.Day.ToString(),
                    StartTime = d.StartTime.ToString("HH:mm"),
                    EndTime = d.EndTime.ToString("HH:mm")
                }).ToList(),
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });

        }


    }
}
