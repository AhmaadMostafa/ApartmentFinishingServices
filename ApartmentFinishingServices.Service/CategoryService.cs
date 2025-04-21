using ApartmentFinishingServices.Core;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Services.Contract;
using ApartmentFinishingServices.Core.Specifications.Category_Specs;
using ApartmentFinishingServices.Core.Specifications.Service_specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Category>> GetCategoriesAsync(CategorySpecParams specParams)
        {
            var spec = new CategoryWithIncludesSpecifications(specParams);
            var categories = await _unitOfWork.Repository<Category>().GetAllWithSpec(spec);
            return categories;
        }

        public async Task<Category?> GetCategoryAsync(int categoryId)
        {
            var spec = new CategoryWithIncludesSpecifications(categoryId);
            var category = await _unitOfWork.Repository<Category>().GetByIdWithSpec(spec);
            return category;
        }

        public async Task<int> GetCategoryCountAsync(CategorySpecParams specParams)
        {
            var countSpec = new CategoryForCountSpecification(specParams);
            var count = await _unitOfWork.Repository<Category>().GetCount(countSpec);
            return count;
        }
        public async Task<IReadOnlyList<Services>> GetServicesAsync(ServiceSpecParams specParams)
        {
            var spec = new ServiceWithIncludesSpecifications(specParams);
            var services = await _unitOfWork.Repository<Services>().GetAllWithSpec(spec);
            return services;
        }
        public async Task<Services?> GetServiceAsync(int serviceId)
        {
            var spec = new ServiceWithIncludesSpecifications(serviceId);
            var service = await _unitOfWork.Repository<Services>().GetByIdWithSpec(spec);
            return service;
        }
        public async Task<int> GetServiceCountAsync(ServiceSpecParams specParams)
        {
            var spec = new ServiceForCountSpecification(specParams);
            var count = await _unitOfWork.Repository<Services>().GetCount(spec);
            return count;
        }

    }
}
