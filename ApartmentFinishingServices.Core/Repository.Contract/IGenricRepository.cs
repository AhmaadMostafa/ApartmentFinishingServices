using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Core.Repository.Contract
{
    public interface IGenricRepository<T> where T: BaseEntity
    {
        Task<IReadOnlyList<T>> GetAll();
        Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec);
        Task<T?> GetByIdWithSpec(ISpecification<T> spec);
        Task<int> GetCount(ISpecification<T> spec);
        Task<T?>GetById(int id);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
