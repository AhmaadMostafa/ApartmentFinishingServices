﻿using ApartmentFinishingServices.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Entities
{
    public class PortfolioItem : BaseEntity
    {
        public string Name { get; set; } 
        public string? Description { get; set; }
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
        public ICollection<PortfolioImage>? PortfolioImages { get; set; } = new HashSet<PortfolioImage>();
    }
}
