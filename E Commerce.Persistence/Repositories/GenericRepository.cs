using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbConext _dbConext;

        public GenericRepository(StoreDbConext dbConext)
        {
            _dbConext = dbConext;
        }
        public async Task AddAsync(TEntity entity) => await _dbConext.Set<TEntity>().AddAsync(entity);

        public async Task<int> CountAsync(ISpecifications<TEntity, TKey> Spec)
        {
            return await SpecificationsEvaluater.CreateQuery(_dbConext.Set<TEntity>(), Spec).CountAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {    
            return await _dbConext.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> Spec)
        {
            return await SpecificationsEvaluater.CreateQuery(_dbConext.Set<TEntity>(), Spec).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id) => await _dbConext.Set<TEntity>().FindAsync(id);

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> Spec)
        {
            return await SpecificationsEvaluater.CreateQuery(_dbConext.Set<TEntity>(), Spec).FirstOrDefaultAsync();
        }

        public void Remove(TEntity entity) => _dbConext.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity) => _dbConext.Set<TEntity>().Update(entity);
    }
}
