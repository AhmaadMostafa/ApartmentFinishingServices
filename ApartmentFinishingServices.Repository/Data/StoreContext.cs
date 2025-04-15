using ApartmentFinishingServices.Core.Entities.Identity;
using ApartmentFinishingServices.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ApartmentFinishingServices.Repository.Data
{
    public class StoreContext : IdentityDbContext<AppUser , IdentityRole<int>,  int>
    {
        public StoreContext(DbContextOptions<StoreContext> options): base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<PortfolioItem> PortfolioItems { get; set; }
        public DbSet<PortfolioImage> PortfolioImages { get; set; }
        public DbSet<AvailableDay> AvailableDays { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SavedWorker> SavedWorkers { get; set; }
    }
}
