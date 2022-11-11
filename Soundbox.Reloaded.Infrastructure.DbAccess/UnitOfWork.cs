namespace Soundbox.Reloaded.Infrastructure.DbAccess
{
    using System;
    using System.Threading.Tasks;
    
    using Soundbox.Reloaded.Core.Domain.Sounds;
    using Soundbox.Reloaded.Infrastructure.DbAccess.Domain.Sounds;

    public class UnitOfWork : IDisposable
    {
        private readonly EntityFrameworkContext context;

        public UnitOfWork()
        {
            this.context = new EntityFrameworkContext();

            // Initialize repositories here
            this.SoundRepository = new SoundRepository(context);
            this.SoundCategoryRepository = new SoundCategoryRepository(context);
        }
        
        // Register public repositories here
        public ISoundRepository SoundRepository { get; set; }
        public ISoundCategoryRepository SoundCategoryRepository { get; set; }

        public int Commit()
        {
            return this.context.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return this.context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
