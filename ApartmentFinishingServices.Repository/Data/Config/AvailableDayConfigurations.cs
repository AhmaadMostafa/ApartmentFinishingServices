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
    internal class AvailableDayConfigurations : IEntityTypeConfiguration<AvailableDay>
    {
        public void Configure(EntityTypeBuilder<AvailableDay> builder)
        {
            // Configure the many-to-one relationship with Worker
            builder.HasOne(a => a.Worker)
                .WithMany(w => w.AvailableDays)
                .HasForeignKey(a => a.WorkerId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Configure the Day enum property
            builder.Property(a => a.Day)
                .HasConversion(
                    d => d.ToString(),  // Convert enum to string for storage
                    d => (Days)Enum.Parse(typeof(Days), d)  // Convert string back to enum
                );

            // Configure TimeOnly properties
            builder.Property(a => a.StartTime)
                .HasConversion(
                    t => t.ToTimeSpan(),
                    t => TimeOnly.FromTimeSpan(t));

            builder.Property(a => a.EndTime)
                .HasConversion(
                    t => t.ToTimeSpan(),
                    t => TimeOnly.FromTimeSpan(t));

        }
    }
}
