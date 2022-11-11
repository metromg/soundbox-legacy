using System;

namespace Soundbox.Reloaded.Ui.Presentation.Dto.Sounds
{
    public class SoundDto : EntityBaseDto
    {
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Title { get; set; }

        public string ArtistName { get; set; }

        public string SoundFileName { get; set; }

        public string ImageFileName { get; set; }
    }
}
