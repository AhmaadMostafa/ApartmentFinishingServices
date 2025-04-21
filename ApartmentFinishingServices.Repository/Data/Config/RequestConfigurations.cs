using ApartmentFinishingServices.Core.Entities;
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
    internal class RequestConfigurations : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            //builder.HasOne(p => p.Worker)
            //    .WithMany(r => r.Requests)
            //    .HasForeignKey(r => r.WorkerId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(p => p.Service)
            //    .WithMany(r => r.Requests)
            //    .HasForeignKey(s => s.ServiceId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(r => r.Customer)
            //    .WithMany(r => r.Requests)
            //    .HasForeignKey(r => r.CustomerId)
            //    .OnDelete(DeleteBehavior.NoAction);

            builder.Property(c => c.Comment).HasMaxLength(200);

            builder.Property(w => w.CustomerSuggestedPrice).HasColumnType("decimal(18,2)");

            builder.Property(w => w.WorkerSuggestedPrice).HasColumnType("decimal(18,2)");

            builder.Property(w => w.FinalAgreedPrice).HasColumnType("decimal(18,2)");

            builder.Property(r => r.Status).HasConversion<string>();
        }
    }
}
