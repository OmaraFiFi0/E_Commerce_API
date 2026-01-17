using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Presistence.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = []; // Using Collection initilization 
       //private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public IGenericRepository<TEntity, TKey> GenericRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>, new()
        {
            // IF Generic Repository Here 

            var EntityType = typeof(TEntity);
            if(_repositories.TryGetValue(EntityType,out var repositories))
            {
                return (IGenericRepository<TEntity, TKey>)repositories;
            }

            // IF TEntity Not Here
            var newRepo = new GenericRepository<TEntity,TKey>(_dbContext);
            _repositories[EntityType] = newRepo;
            return newRepo;
        }
    }
}
