
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
            builder.Services.AddScoped(typeof(IGenricRepository<>), typeof(GenricRepository<>));

            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy => policy.WithOrigins("http://localhost:5173")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
