using ApartmentFinishingServices.Core;
using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Entities.Identity;
using ApartmentFinishingServices.Core.Services.Contract;
using ApartmentFinishingServices.Core.Specifications.Admin_Specs;
using ApartmentFinishingServices.Core.Specifications.Customer_Specs;
using ApartmentFinishingServices.Core.Specifications.Request_specs;
using ApartmentFinishingServices.Core.Specifications.Worker_Specs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Service
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public AdminService(IUnitOfWork unitOfWork , UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        #region User Management
        public async Task<IReadOnlyList<Worker>> GetAllWorkersAsync(WorkerSpecParams specParams)
        {
            var spec = new WorkerWithDetailsSpecification(specParams);
            return await _unitOfWork.Repository<Worker>().GetAllWithSpec(spec);
        }
        public async Task<IReadOnlyList<Customer>> GetAllCustomersAsync(CustomerSpecParams specParams)
        {
            var spec = new CustomerWithDetailsSpecification(specParams);
            return await _unitOfWork.Repository<Customer>().GetAllWithSpec(spec);
        }

        public async Task<int> GetCustomersCountAsync(CustomerSpecParams specParams)
        {
            var countSpec = new CustomerForCountSpecification(specParams);
            return await _unitOfWork.Repository<Customer>().GetCount(countSpec);
        }

        public async Task<int> GetWorkersCountAsync(WorkerSpecParams specParams)
        {
            var countSpec = new WorkerForCountSpecification(specParams);
            return await _unitOfWork.Repository<Worker>().GetCount(countSpec);
        }
        public async Task<Worker?> GetWorkerAsync(int workerId)
        {
            var spec = new WorkerWithDetailsSpecification(workerId);
            return await _unitOfWork.Repository<Worker>().GetByIdWithSpec(spec);
        }

        public async Task<Customer?> GetCustomerAsync(int customerId)
        {
            var spec = new CustomerWithDetailsSpecification(customerId);
            return await _unitOfWork.Repository<Customer>().GetByIdWithSpec(spec);
        }
        #endregion

        #region Request Management
        public async Task<IReadOnlyList<Request>> GetAllRequestsAsync(RequestSpecParams specParams)
        {
            var spec = new RequestWithFilterSpecification(specParams);
            return await _unitOfWork.Repository<Request>().GetAllWithSpec(spec);
        }

        public async Task<int> GetRequestsCountAsync(RequestSpecParams specParams)
        {
            var countSpec = new RequestForCountSpecification(specParams);
            return await _unitOfWork.Repository<Request>().GetCount(countSpec);
        }

        public Task<Dictionary<RequestStatus, int>> GetRequestStatusStatisticsAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<Request?> GetRequestAsync(int requestId)
        {
            var spec = new RequestWithDetailsSpecification(requestId);
            return await _unitOfWork.Repository<Request>().GetByIdWithSpec(spec);
        }
        #endregion

        #region Category Management
        public async Task<Category> AddCategoryAsync(string name, string? pictureUrl)
        {
            var category = new Category
            {
                Name = name,
                PictureUrl = pictureUrl
            };
            await _unitOfWork.Repository<Category>().Add(category);
            await _unitOfWork.CompleteAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var category = await _unitOfWork.Repository<Category>().GetById(categoryId);

            if (category == null)
                return false;

            _unitOfWork.Repository<Category>().Delete(category);
            await _unitOfWork.CompleteAsync();

            return true;
        }
        #endregion

        #region Service Management
        public async Task<Services> AddServiceAsync(string name, int categoryId)
        {
            var service = new Services
            {
                Name = name,
                CategoryId = categoryId
            };
            await _unitOfWork.Repository<Services>().Add(service);
            await _unitOfWork.CompleteAsync();
            return service;
        }

        public async Task<bool> DeleteServiceAsync(int serviceId)
        {
            var service = await _unitOfWork.Repository<Services>().GetById(serviceId);
            if (service == null) return false;

            var requestsSpec = new RequestsByServiceIdSpecification(serviceId);
            var hasRequests = await _unitOfWork.Repository<Request>().GetCount(requestsSpec);

            if (hasRequests > 0)
                return false;

            _unitOfWork.Repository<Services>().Delete(service);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> LockUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            if (result.Succeeded)
            {
                user.IsBlocked = true;
                var updateResult = await _userManager.UpdateAsync(user);
                return updateResult.Succeeded;
            }
            return result.Succeeded;
        }

        public async Task<bool> UnlockUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            Console.WriteLine("UserID: " + userId);
            if (user == null) return false;

            var result = await _userManager.SetLockoutEndDateAsync(user, null);
            if (result.Succeeded)
            {
                user.IsBlocked = false;
                var updateResult = await _userManager.UpdateAsync(user);
                return updateResult.Succeeded;
            }

            return result.Succeeded;
        }

        #endregion
    }
}
