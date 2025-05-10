using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Entities.Identity;
using ApartmentFinishingServices.Core.Specifications.Customer_Specs;
using ApartmentFinishingServices.Core.Specifications.Request_specs;
using ApartmentFinishingServices.Core.Specifications.Worker_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service = ApartmentFinishingServices.Core.Entities.Services;


namespace ApartmentFinishingServices.Core.Services.Contract
{
    public interface IAdminService
    {
        #region User Management
        Task<IReadOnlyList<Worker>> GetAllWorkersAsync(WorkerSpecParams specParams);
        Task<int> GetWorkersCountAsync(WorkerSpecParams specParams);
        Task<Worker?> GetWorkerAsync(int workerId);
        Task<Customer?> GetCustomerAsync(int customerId);
        Task<IReadOnlyList<Customer>> GetAllCustomersAsync(CustomerSpecParams specParams);
        Task<int> GetCustomersCountAsync(CustomerSpecParams specParams);
        Task<bool> LockUserAsync(int userId);
        Task<bool> UnlockUserAsync(int userId);

        //Task<IReadOnlyList<AppUser>> GetBlockedUsersAsync();
        #endregion

        #region Request Management
        Task<IReadOnlyList<Request>> GetAllRequestsAsync(RequestSpecParams specParams);
        Task<int> GetRequestsCountAsync(RequestSpecParams specParams);
        Task<Request?> GetRequestAsync(int requestId);
        Task<Dictionary<RequestStatus, int>> GetRequestStatusStatisticsAsync();
        #endregion

        #region Category Management
        Task<Category>AddCategoryAsync(string name , string? pictureUrl);
        Task<bool>DeleteCategoryAsync(int categoryId);
        #endregion

        #region Service Management
        Task<Service> AddServiceAsync(string name, int categoryId);
        Task<bool> DeleteServiceAsync(int serviceId);
        #endregion


    }
}
