using ApartmentFinishingServices.APIs.Dtos;
using ApartmentFinishingServices.APIs.Dtos.AdminDtos;
using ApartmentFinishingServices.APIs.Errors;
using ApartmentFinishingServices.APIs.Extenstions;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Entities.Identity;
using ApartmentFinishingServices.Core.Repository.Contract;
using ApartmentFinishingServices.Core.Services.Contract;
using ApartmentFinishingServices.Core.Specifications.Admin_Specs;
using ApartmentFinishingServices.Core.Specifications.Request_specs;
using ApartmentFinishingServices.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApartmentFinishingServices.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IGenricRepository<Customer> _customerRepo;
        private readonly IGenricRepository<Worker> _workerRepo;
        private readonly IGenricRepository<City> _cityRepo;
        private readonly IGenricRepository<Admin> _adminRepo;
        private readonly IGenricRepository<AvailableDay> _availableDayRepo;
        private readonly IGenricRepository<Services> _serviceRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IFileService _fileService;
        private readonly string[] _allowedProfilePictureExtensions = new[] { ".jpg", ".jpeg", ".png" };


        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IAuthService authService , IGenricRepository<Customer> customerRepo , IGenricRepository<City> cityRepo
            , IGenricRepository<Worker> workerRepo , IGenricRepository<Services> serviceRepo, IMapper mapper
            , IGenricRepository<AvailableDay> availableDayRepo , IWebHostEnvironment environment , IFileService fileService
            ,IGenricRepository<Admin> adminRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _cityRepo = cityRepo;
            _serviceRepo = serviceRepo;
            _availableDayRepo = availableDayRepo;
            _environment = environment;
            _fileService = fileService;
            _adminRepo = adminRepo;
            _customerRepo = customerRepo;
            _workerRepo = workerRepo;
            _mapper = mapper;
        }
        private async Task<ActionResult<AppUser>> RegisterUser(RegisterBaseDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new[] { "This email is already exist!!" } });

            if (model.Password != model.ConfirmPassword)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new[] { "Password do not match" } });

            var cityExist = await _cityRepo.GetById(model.CityId);
            if (cityExist is null)
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Invalid city" } });

            string profilePictureUrl = null;

            if (model.ProfilePicture != null)
            {
                try
                {
                    var fileName = await _fileService.SaveFileAsync(model.ProfilePicture, _allowedProfilePictureExtensions);
                    profilePictureUrl = $"https://localhost:7118/resources/{fileName}";
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(new ApiValidationErrorResponse { Errors = new[] { ex.Message } });
                }
                catch (Exception ex)
                {
                    return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Error uploading profile picture" } });
                }
            }
            var user = new AppUser()
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                CityId = model.CityId,
                Age = model.Age,
                ProfilePictureUrl = profilePictureUrl,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded is false)
            {
                return BadRequest(new ApiResponse(400));
            }
            return user;
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateProfile([FromForm] UpdateProfileDto model)
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            if (user == null)
                return NotFound(new ApiResponse(404, "User not found"));

            if (model.CityId.HasValue)
            {
                var cityExists = await _cityRepo.GetById(model.CityId.Value);
                if (cityExists == null)
                    return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Invalid city" } });
            }   

            if (!string.IsNullOrEmpty(model.Name))
                user.Name = model.Name;

            if (!string.IsNullOrEmpty(model.PhoneNumber))
                user.PhoneNumber = model.PhoneNumber;

            if (!string.IsNullOrEmpty(model.Address))
                user.Address = model.Address;

            if (model.CityId.HasValue)
                user.CityId = model.CityId.Value;

            if (model.Age.HasValue)
                user.Age = model.Age.Value;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse { Errors = result.Errors.Select(e => e.Description).ToArray() });

            return Ok(new ApiResponse(200, "Profile updated successfully"));
        }


        [HttpPost("register/customer")]
        public async Task<ActionResult<CustomerToReturnDto>> RegisterAsCustomer(RegisterAsCustomerDto model)
        {
            var registrationResult = await RegisterUser(model);
            if (registrationResult.Result is BadRequestObjectResult badRequest)
                return BadRequest(badRequest.Value);
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
                return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            var cityExist = await _cityRepo.GetById(user.CityId);
            if (result.Succeeded is false)
                return Unauthorized(new ApiResponse(401));
            var roles = await _userManager.GetRolesAsync(user);

            var userInfo = new UserResponseBaseDto()
            {
                DisplayName = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CityName = cityExist?.Name,
                Age = user.Age,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            };
            if(roles.Contains("Worker"))
            {
                var worker = await _workerRepo.GetByIdWithSpec(new WorkerByAppUserIdSpecification(user.Id));
                if(worker != null)
                {
                    var workerDto = _mapper.Map<WorkerToReturnDtoWithEarnings>(userInfo);
                    workerDto.TotalEarnings = worker.TotalEarnings;
                    return Ok(workerDto);
                }
            }
            else if (roles.Contains("Admin"))
            {
                var admin = await _adminRepo.GetByIdWithSpec(new AdminByAppUserIdSpecification(user.Id));
                if(admin != null)
                {
                    var adminDto = _mapper.Map<AdminToReturnDto>(userInfo);
                    adminDto.TotalEarnings = admin.TotalEarnings;
                    var customerSpecParams = new Core.Specifications.Customer_Specs.CustomerSpecParams();
                    var workerSpecParams = new Core.Specifications.Worker_Specs.WorkerSpecParams();

                    var totalCustomers = await _customerRepo.GetCount();
                    var totalWorkers = await _workerRepo.GetCount();

                    adminDto.TotalCustomers = totalCustomers;
                    adminDto.TotalWorkers = totalWorkers;

                    return Ok(adminDto);
                }
            }
            return Ok(userInfo);

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
            if (registrationResult.Result is BadRequestObjectResult badRequest)
                return BadRequest(badRequest.Value);

            var user = registrationResult.Value;
            var serviceExist = await _serviceRepo.GetById(model.ServiceId);

            if (serviceExist is null)
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Invalid service" } });

            var worker = new Worker()
            {
                Description = string.IsNullOrEmpty(model.Description) ? "New Worker" : model.Description,
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

        [HttpDelete("delete-profile-picture")]
        [Authorize]
        public async Task<ActionResult> DeleteProfilePicture()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found"));
            }

            if (string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                return BadRequest(new ApiResponse(400, "User doesn't have a profile picture"));
            }
            try
            {
                var fileName = Path.GetFileName(user.ProfilePictureUrl);

                _fileService.DeleteFile(fileName);

                user.ProfilePictureUrl = null;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(new ApiResponse(400, "Failed to update user profile"));
                }

                return Ok(new ApiResponse(200, "Profile picture deleted successfully"));
            }
            catch (FileNotFoundException)
            {
                user.ProfilePictureUrl = null;
                await _userManager.UpdateAsync(user);
                return BadRequest(new ApiResponse(400, "Profile picture file not found (URL cleared)"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, $"Error deleting profile picture: {ex.Message}"));
            }
        }
        [HttpPut("update-profile-picture")]
        [Authorize]
        public async Task<ActionResult<string>> UpdateProfilePicture([FromForm] IFormFile newProfilePicture)
        {
            if (newProfilePicture == null || newProfilePicture.Length == 0)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "No file was uploaded." } });
            }

            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found"));
            }

            try
            {
                if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                {
                    var oldFileName = Path.GetFileName(user.ProfilePictureUrl);
                    try
                    {
                        _fileService.DeleteFile(oldFileName);
                    }
                    catch (FileNotFoundException)
                    {

                    }
                }

                var fileName = await _fileService.SaveFileAsync(newProfilePicture, _allowedProfilePictureExtensions);
                user.ProfilePictureUrl = $"https://localhost:7118/resources/{fileName}";

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(new ApiResponse(400, "Failed to update user profile"));
                }

                return Ok(user.ProfilePictureUrl);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { ex.Message } });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, $"Error updating profile picture: {ex.Message}"));
            }
        }

    }
}
