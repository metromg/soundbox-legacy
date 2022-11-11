namespace Soundbox.Reloaded.Infrastructure.DbAccess.Domain.Sounds
{
    using System.Linq;
    using Soundbox.Reloaded.Core.Domain.Sounds;
    using Soundbox.Reloaded.Core.Domain.Sounds.Model;
    using System.Data.Entity;

    public class SoundRepository : Repository<Sound>, ISoundRepository
    {
        public SoundRepository(EntityFrameworkContext context)
            : base(context)
        {
        }

        protected override IQueryable<Sound> GetQuery()
        {
            return base.GetQuery().Include(e => e.Category);
        }
    }
}
