namespace Soundbox.Reloaded.Infrastructure.DbAccess.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using Soundbox.Reloaded.Core.Domain;

    internal abstract class MappingBase<TEntity> : EntityTypeConfiguration<TEntity>
        where TEntity : EntityBase
    {
        public MappingBase()
        {
            this.HasKey(e => e.Id);
        }
    }
}
