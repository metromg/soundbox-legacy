namespace Soundbox.Reloaded.Core.Domain.Sounds.Objects
{
    using System;
    using System.IO;
    
    using NAudio.Wave;

    using Soundbox.Reloaded.Core.Domain.Sounds.Model;
    using Soundbox.Reloaded.Core.Properties;

    public class PlayableSound
    {
        private DateTime soundEnd;

        public PlayableSound(Sound sound)
        {
            this.Sound = sound;
            this.Reset(sound.SoundFileName);
        }

        public Sound Sound { get; set; }

        public bool IsPlaying { get; set; }

        public int CurrentTime { get; set; }

        public int Duration { get; set; }

        public DateTime SoundEnd
        {
            get { return soundEnd; }
        }

        public void Reset(string soundFileName)
        {
            var reader = PrepareSoundFileReader(soundFileName);

            this.IsPlaying = false;
            this.CurrentTime = 0;
            this.Duration = (int)reader.TotalTime.TotalSeconds;

            reader.Dispose();
        }

        public void Play()
        {
            this.SetSoundEnd();
            this.IsPlaying = true;
        }

        public void Pause()
        {
            this.IsPlaying = false;
        }

        public void Seek(int seconds)
        {
            this.CurrentTime = seconds;
            this.SetSoundEnd();
        }

        private static Mp3FileReader PrepareSoundFileReader(string soundFileName)
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var soundFileDirectory = new DirectoryInfo(Path.Combine(rootPath, "SoundFiles"));

            if (!soundFileDirectory.Exists)
            {
                throw new DirectoryNotFoundException(Messages.SoundFileDirectoryDoesNotExist);
            }

            return new Mp3FileReader(Path.Combine(soundFileDirectory.FullName, soundFileName));
        }
        
        private void SetSoundEnd()
        {
            this.soundEnd = DateTime.Now.AddSeconds(this.Duration - this.CurrentTime);
        }
    }
}
