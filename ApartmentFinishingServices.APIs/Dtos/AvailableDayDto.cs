using ApartmentFinishingServices.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApartmentFinishingServices.APIs.Dtos
{
    public class AvailableDayDto
    {
        [Required]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        public Days Day { get; set; }

        [Required]
        //[RegularExpression(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$")]
        public string StartTime { get; set; } // Format: "HH:mm"

        [Required]
        //[RegularExpression(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$")]
        public string EndTime { get; set; } // Format: "HH:mm"
    }
}