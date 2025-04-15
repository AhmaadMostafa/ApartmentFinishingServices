using ApartmentFinishingServices.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Repository.Data.Config
{
    internal class SavedWorkerConfigurations : IEntityTypeConfiguration<SavedWorker>
    {
        public void Configure(EntityTypeBuilder<SavedWorker> builder)
        {
            // Configure the relationship with Customer
            builder.HasOne(sw => sw.Customer)
                .WithMany(c => c.SavedWorkers)
                .HasForeignKey(sw => sw.CustomerId)
                .OnDelete(DeleteBehavior.NoAction); 

            // Configure the relationship with Worker
            builder.HasOne(sw => sw.Worker)
                .WithMany(w => w.SavedWorkers)
                .HasForeignKey(sw => sw.ProviderId)
                .OnDelete(DeleteBehavior.NoAction); 

            // Configure unique constraint to prevent duplicate saves
            builder.HasIndex(sw => new { sw.CustomerId, sw.ProviderId }).IsUnique();

            // Optional: Property configurations
            builder.Property(sw => sw.CustomerId)
                .IsRequired();

            builder.Property(sw => sw.ProviderId)
                .IsRequired();
        }
    }
}
