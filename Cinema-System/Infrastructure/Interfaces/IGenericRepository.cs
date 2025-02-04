using Ardalis.Specification;

namespace Infrastructure.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity? GetById(int id);
        void Insert(TEntity entity);
        void Delete(int id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        TEntity? GetFirstBySpec(ISpecification<TEntity> specification);
        IEnumerable<TEntity> GetListBySpec(ISpecification<TEntity> specification);
    }
}