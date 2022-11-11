namespace Soundbox.Reloaded.Ui.Presentation.Dto.Sounds
{
    public class PlayableSoundDto
    {
        public SoundDto Sound { get; set; }

        public bool IsPlaying { get; set; }

        public int CurrentTime { get; set; }

        public int Duration { get; set; }
    }
}
