namespace Soundbox.Reloaded.Infrastructure.Services
{
    using System;
    using System.Timers;

    using Soundbox.Reloaded.Core.Domain.Sounds.Model;
    using Soundbox.Reloaded.Core.Domain.Sounds.Objects;

    public delegate void SoundEndsEventHandler(object sender, EventArgs e);

    public class SoundPlaybackService
    {
        public event SoundEndsEventHandler SoundEnds;

        private readonly Timer playTime;
        private PlayableSound currentSound;

        public SoundPlaybackService()
        {
            this.playTime = new Timer();
            this.playTime.Interval = 1000;
            this.playTime.Elapsed += new ElapsedEventHandler(this.IncrementCurrentSoundTimeEvent);

            this.currentSound = default(PlayableSound);
        }

        public bool IsPlaying
        {
            get { return this.currentSound != null && this.currentSound.IsPlaying; }
        }

        public PlayableSound CurrentSoundOrDefault()
        {
            return this.currentSound;
        }

        public void PlaySoundOrStopIfNull(Sound sound)
        {
            this.playTime.Stop();

            if (sound == null)
            {
                if (this.currentSound != null)
                {
                    this.currentSound.Pause();
                }

                this.currentSound = default(PlayableSound);
                return;
            }

            this.currentSound = new PlayableSound(sound);
            this.currentSound.Play();

            this.playTime.Start();
        }

        public void PauseSound()
        {
            if (this.currentSound == null)
            {
                return;
            }

            this.currentSound.Pause();

            this.playTime.Stop();
        }

        public void ContinueSound()
        {
            if (this.currentSound == null)
            {
                return;
            }
            
            this.currentSound.Play();

            this.playTime.Start();
        }

        public void SeekSound(int seconds)
        {
            if (this.currentSound == null)
            {
                return;
            }
            
            this.currentSound.Seek(seconds);
        }

        private void IncrementCurrentSoundTimeEvent(object source, ElapsedEventArgs e)
        {
            if (DateTime.Now >= this.currentSound.SoundEnd)
            {
                this.playTime.Stop();

                this.currentSound.Pause();
                this.currentSound = default(PlayableSound);
                
                if (this.SoundEnds != null)
                {
                    SoundEnds(this, EventArgs.Empty);
                }

                return;
            }

            var currentToEndTimespan = this.currentSound.SoundEnd.Subtract(DateTime.Now);
            this.currentSound.CurrentTime = this.currentSound.Duration - (int)currentToEndTimespan.TotalSeconds;
        }
    }
}
