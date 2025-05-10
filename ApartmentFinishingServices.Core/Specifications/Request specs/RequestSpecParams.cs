using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Request_specs
{
    public class RequestSpecParams
    {
        private const int MaxPageSize = 10;
        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; }

        private string? search;
        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
        public int? Status { get; set; }
        public int? WorkerId { get; set; }
        public int? CustomerId { get; set; }
        public int? ServiceId { get; set; }

    }
}
