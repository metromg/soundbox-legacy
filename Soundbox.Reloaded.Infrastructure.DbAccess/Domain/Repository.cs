namespace Soundbox.Reloaded.Infrastructure.DbAccess
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Data.Entity;
    using System.Linq;
    
    using Soundbox.Reloaded.Core.Domain;

    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : EntityBase
    {
        private readonly DbSet<TEntity> set;

        protected Repository(EntityFrameworkContext context)
        {
            this.set = context.Set<TEntity>();
        }

        public TEntity Create()
        {
            return this.set.Create();
        }

        public void Add(TEntity entity)
        {
            this.set.Add(entity);
        }
        
        public void RemoveById(Guid id)
        {
            var toRemove = this.set.Find(id);
            if (toRemove == null)
            {
                return;
            }

            this.Remove(toRemove);
        }

        public void Remove(TEntity entity)
        {
            this.set.Remove(entity);
        }

        public TEntity GetById(Guid id)
        {
            return this.GetQuery().Single(e => e.Id == id);
        }

        public Task<TEntity> GetByIdAsync(Guid id)
        {
            return this.GetQuery().SingleAsync(e => e.Id == id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.GetQuery().ToArray();
        }

        public Task<TEntity[]> GetAllAsync()
        {
            return this.GetQuery().ToArrayAsync();
        }

        protected DbSet<TEntity> GetSet()
        {
            return this.set;
        }

        protected virtual IQueryable<TEntity> GetQuery()
        {
            return this.set;
        }
    }
}
