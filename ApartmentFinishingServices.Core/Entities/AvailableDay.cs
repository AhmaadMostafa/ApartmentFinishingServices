using ApartmentFinishingServices.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Entities
{
    public enum Days
    {
        Saturday,
        Sunday,
        Monday,
        TuesDay,
        Wednesday,
        Thursday,
        Friday
    }

    public class AvailableDay : BaseEntity
    {
        public int WorkerId { get; set; }
        public Days Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public Worker? Worker { get; set; }
    }
}
