using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Specifications.Category_Specs;
using ApartmentFinishingServices.Core.Specifications.Service_specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service = ApartmentFinishingServices.Core.Entities.Services;

namespace ApartmentFinishingServices.Core.Services.Contract
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<Category>> GetCategoriesAsync(CategorySpecParams specParams);
        Task<int> GetCategoryCountAsync(CategorySpecParams specParams);
        Task<Category?> GetCategoryAsync(int categoryId);
        Task<IReadOnlyList<Service>> GetServicesAsync(ServiceSpecParams specParams);
        Task<Service?> GetServiceAsync(int serviceId);
        Task<int> GetServiceCountAsync(ServiceSpecParams specParams);

    }
}
