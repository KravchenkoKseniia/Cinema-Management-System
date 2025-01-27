using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using CMS.Infrastructure.Data;
using CMS.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace CMS.Infrastructure.Repositories
{
    public class GenericRepository<TEntity>(ApplicationDbContext context) : IGenericRepository<TEntity>
        where TEntity : class
    {
        internal readonly ApplicationDbContext Context = context;
        internal readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

        public virtual TEntity? GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(int id)
        {
            TEntity? entityToDelete = DbSet.Find(id);
            if (entityToDelete != null) Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }
        
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this._disposed = true;
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        public TEntity? GetFirstBySpec(ISpecification<TEntity> specification)
        {
            return ApplySpecification(specification).FirstOrDefault();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            var evaluator = new SpecificationEvaluator();
            return evaluator.GetQuery(DbSet, specification);
        }
        
        public IEnumerable<TEntity> GetListBySpec(ISpecification<TEntity> specification)
        {
            return ApplySpecification(specification).ToList();
        }
    }
    
    
}

