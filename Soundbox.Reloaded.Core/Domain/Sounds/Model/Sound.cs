namespace Soundbox.Reloaded.Core.Domain.Sounds.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;

    using Soundbox.Reloaded.Core.Properties;

    public class Sound : EntityBase
    {
        public Guid CategoryId { get; set; }

        public virtual SoundCategory Category { get; set; }

        [Required]
        [StringLength(Constants.StringLengths.NAME)]
        public string Title { get; set; }

        [Required]
        [StringLength(Constants.StringLengths.NAME)]
        public string ArtistName { get; set; }

        [Required]
        [StringLength(Constants.StringLengths.FILENAME)]
        public string SoundFileName { get; set; }
        
        [StringLength(Constants.StringLengths.FILENAME)]
        public string ImageFileName { get; set; }

        public IEnumerable<string> Validate()
        {
            const string supportedSoundExtension = ".mp3";
            const string supportedImageExtension = ".png";

            if (!StringComparer.InvariantCultureIgnoreCase.Equals(Path.GetExtension(this.SoundFileName), supportedSoundExtension))
            {
                yield return string.Format(Messages.NotSupportedSoundFileName, supportedSoundExtension);
            }

            if (!string.IsNullOrWhiteSpace(this.ImageFileName) && !StringComparer.InvariantCultureIgnoreCase.Equals(Path.GetExtension(this.ImageFileName), supportedImageExtension))
            {
                yield return string.Format(Messages.NotSupportedImageFileName, supportedImageExtension);
            }
        }
    }
}
