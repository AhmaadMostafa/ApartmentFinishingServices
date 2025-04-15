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
    internal class ServiceConfigurations : IEntityTypeConfiguration<Services>
    {
        public void Configure(EntityTypeBuilder<Services> builder)
        {
            builder.HasOne(p => p.Category)
                .WithMany(p => p.Services)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
