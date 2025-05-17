
using ApartmentFinishingServices.Core.Entities.Identity;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Repository.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ApartmentFinishingServices.Core.Repository.Contract;
using ApartmentFinishingServices.Repository;
using ApartmentFinishingServices.APIs.Helpers;
using ApartmentFinishingServices.Core.Services.Contract;
using ApartmentFinishingServices.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApartmentFinishingServices.APIs.Extenstions;
using ApartmentFinishingServices.Repository.Identity.IdentitySeed;
using ApartmentFinishingServices.Core;
using Microsoft.AspNetCore.Mvc;
using ApartmentFinishingServices.APIs.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.FileProviders;
using ApartmentFinishingServices.APIs.Hubs;
using Microsoft.AspNetCore.Http.Connections;

namespace ApartmentFinishingServices.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped(typeof(IFileService), typeof(FileService));
            builder.Services.AddScoped(typeof(IAdminService), typeof(AdminService));
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddScoped(typeof(IRequestService), typeof(RequestService));
            builder.Services.AddScoped(typeof(IReviewService), typeof(ReviewService));
            builder.Services.AddScoped(typeof(IGenricRepository<>), typeof(GenricRepository<>));
            builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            builder.Services.AddScoped<IChatService, ChatService>();


            builder.Services.AddSignalR(options => {
                options.EnableDetailedErrors = true;
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
            });
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                         .SelectMany(p => p.Value.Errors)
                                                         .Select(err => err.ErrorMessage).ToArray();
                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy => policy.WithOrigins("http://localhost:5173" , "http://localhost:5174")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials());
            });


            var app = builder.Build();

            app.UseCors("AllowReactApp");

            using var scope = app.Services.CreateScope();


            var services = scope.ServiceProvider;

            var _dbcontext = services.GetRequiredService<StoreContext>();
            // ask clr for creating object from dbcontext explicitly
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbcontext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(_dbcontext);
                var _roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
                var _userManager = services.GetRequiredService<UserManager<AppUser>>();
                var _unitOfWork = services.GetRequiredService<IUnitOfWork>();
                await IdentitySeed.SeedAdminUserAsync(_userManager , _unitOfWork);
                await IdentitySeed.SeedRolesAsync(_roleManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has been occured during apply migration");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath
                , "uploads")),
                RequestPath = "/resources"
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapHub<ChatHub>("/chatHub", options => {
                options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
                options.CloseOnAuthenticationExpiration = true;
                options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(10);
            });

            app.MapControllers();

            app.Run();
        }
    }
}
