using Soundbox.Reloaded.Core.Domain.Sounds.Model;

namespace Soundbox.Reloaded.Infrastructure.DbAccess.Mapping.Sounds
{
    internal class SoundCategoryMapping : MappingBase<SoundCategory>
    {
        public SoundCategoryMapping()
        {
            this.HasMany(e => e.Sounds).WithRequired(e => e.Category).HasForeignKey(e => e.CategoryId).WillCascadeOnDelete(true);
        }
    }
}
