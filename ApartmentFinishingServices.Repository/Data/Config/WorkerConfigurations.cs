using ApartmentFinishingServices.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Repository.Data.Config
{
    internal class WorkerConfigurations : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            // Configure the one-to-one relationship with AppUser
            builder.HasOne(w => w.AppUser)
                .WithOne()
                .HasForeignKey<Worker>(w => w.AppUserId)
                .OnDelete(DeleteBehavior.Cascade); // If AppUser is deleted, delete Worker too

            // Configure the many-to-one relationship with Service
            builder.HasOne(w => w.Service)
                .WithMany(s => s.Workers)
                .HasForeignKey(w => w.ServiceId)
                .OnDelete(DeleteBehavior.SetNull); // If Service is deleted, set ServiceId to null

            // Configure one-to-many relationships
            builder.HasMany(w => w.PortfolioItems)
                .WithOne(p => p.Worker)
                .HasForeignKey(p => p.WorkerId)
                .OnDelete(DeleteBehavior.Cascade); // Delete portfolio items when worker is deleted

            //builder.HasMany(w => w.Requests)
            //    .WithOne(r => r.Worker)
            //    .HasForeignKey(r => r.WorkerId)
            //    .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if requests exist

            builder.Property(w => w.Description)
                .HasMaxLength(500);

            builder.Property(w => w.MinPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(w => w.MaxPrice)
                .HasColumnType("decimal(18,2)");
        }

    }
}
