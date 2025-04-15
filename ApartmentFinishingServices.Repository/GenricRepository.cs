using ApartmentFinishingServices.Core.Entities;
using ApartmentFinishingServices.Core.Repository.Contract;
using ApartmentFinishingServices.Core.Specifications;
using ApartmentFinishingServices.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Repository
{
    public class GenricRepository<T> : IGenricRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenricRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task Add(T entity) => await _dbContext.AddAsync(entity);
        
        public async Task<T?> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public void Update(T entity) => _dbContext.Update(entity);
        public void Delete(T entity) => _dbContext.Remove(entity);

        public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<T?> GetByIdWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }
        private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>() , spec);
        }

        public async Task<int> GetCount(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }

    }
}
