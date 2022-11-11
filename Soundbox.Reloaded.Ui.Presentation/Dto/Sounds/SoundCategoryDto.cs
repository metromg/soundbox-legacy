namespace Soundbox.Reloaded.Ui.Presentation.Dto.Sounds
{
    using System.Collections.Generic;

    public class SoundCategoryDto : EntityBaseDto
    {
        public string Name { get; set; }

        public ICollection<SoundDto> Sounds { get; set; }
    }
}
