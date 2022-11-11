namespace Soundbox.Reloaded.Infrastructure.DbAccess
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Threading.Tasks;
    using Soundbox.Reloaded.Core.Domain.Sounds;
    using Soundbox.Reloaded.Core.Domain.Sounds.Model;
    using Soundbox.Reloaded.Infrastructure.DbAccess.Domain.Sounds;
    using Soundbox.Reloaded.Infrastructure.DbAccess.Mapping.Sounds;

    public class EntityFrameworkContext : DbContext
    {
        public EntityFrameworkContext()
            : base("name=app")
        {
        }

        // Register private dbsets here
        private DbSet<Sound> Sounds { get; set; }
        private DbSet<SoundCategory> SoundCategories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Register mappings
            modelBuilder.Configurations.Add(new SoundMapping());
            modelBuilder.Configurations.Add(new SoundCategoryMapping());
        }
    }
}
