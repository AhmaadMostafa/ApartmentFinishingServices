using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Specifications.Category_Specs
{
    public class CategorySpecParams
    {
        private const int MaxPageSize = 10;
        private int pageSize = 6;
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
    }
}
