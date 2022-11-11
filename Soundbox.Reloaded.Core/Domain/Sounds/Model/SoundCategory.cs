namespace Soundbox.Reloaded.Core.Domain.Sounds.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;

    using Soundbox.Reloaded.Core.Properties;

    public class SoundCategory : EntityBase
    {
        public SoundCategory()
        {
            this.Sounds = new Collection<Sound>();
        }

        [Required]
        [StringLength(Constants.StringLengths.NAME)]
        public string Name { get; set; }

        public virtual ICollection<Sound> Sounds { get; set; }

        public IEnumerable<string> Validate()
        {
            if (this.Sounds.Count == 0)
            {
                yield return Messages.SoundOnCategoryMissing;
            }
        }
    }
}
