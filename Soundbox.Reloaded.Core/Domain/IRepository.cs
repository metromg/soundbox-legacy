namespace Soundbox.Reloaded.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<TEntity>
        where TEntity : EntityBase
    {
        TEntity Create();

        void Add(TEntity entity);

        void RemoveById(Guid id);

        void Remove(TEntity entity);
        
        TEntity GetById(Guid id);

        Task<TEntity> GetByIdAsync(Guid id);
        
        IEnumerable<TEntity> GetAll();

        Task<TEntity[]> GetAllAsync();
    }
}
