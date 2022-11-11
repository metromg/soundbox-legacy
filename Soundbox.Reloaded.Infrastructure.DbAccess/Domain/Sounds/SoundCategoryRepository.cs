namespace Soundbox.Reloaded.Infrastructure.DbAccess.Domain.Sounds
{
    using Soundbox.Reloaded.Core.Domain.Sounds;
    using Soundbox.Reloaded.Core.Domain.Sounds.Model;
    using System.Data.Entity;
    using System.Linq;

    public class SoundCategoryRepository : Repository<SoundCategory>, ISoundCategoryRepository
    {
        public SoundCategoryRepository(EntityFrameworkContext context)
            : base(context)
        {
        }

        protected override IQueryable<SoundCategory> GetQuery()
        {
            return base.GetQuery().Include(e => e.Sounds);
        }
    }
}
