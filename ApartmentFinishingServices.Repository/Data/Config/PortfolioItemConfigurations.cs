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
    internal class PortfolioItemConfigurations : IEntityTypeConfiguration<PortfolioItem>
    {
        public void Configure(EntityTypeBuilder<PortfolioItem> builder)
        {
            // Configure the many-to-one relationship with Worker
            builder.HasOne(p => p.Worker)
                .WithMany(w => w.PortfolioItems)
                .HasForeignKey(p => p.WorkerId);

            // Configure the one-to-many relationship with PortfolioImages
            builder.HasMany(p => p.PortfolioImages)
                .WithOne(pi => pi.PortfolioItem)
                .HasForeignKey(pi => pi.PortfolioId)
                .OnDelete(DeleteBehavior.Cascade); // Delete images when portfolio item is deleted

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(500);
        }
    }
}
