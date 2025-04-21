using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Entities
{
    public enum RequestStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Accepted")]
        Accepted,
        [EnumMember(Value = "Rejected")]
        Rejected,
        [EnumMember(Value = "In Progress")]
        InProgress,
        [EnumMember(Value = "Completed")]
        Completed
    }
}
