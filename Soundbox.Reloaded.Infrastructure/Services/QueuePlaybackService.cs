namespace Soundbox.Reloaded.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Soundbox.Reloaded.Core.Domain.Sounds.Model;
    using Soundbox.Reloaded.Core.Domain.Sounds.Objects;

    public class QueuePlaybackService
    {
        private bool isLooping;
        private int currentQueuePosition;
        private List<Sound> queue;

        private static QueuePlaybackService queuePlaybackService;

        private QueuePlaybackService()
        {
            this.SoundPlaybackService = new SoundPlaybackService();
            this.SoundPlaybackService.SoundEnds += new SoundEndsEventHandler((object sender, EventArgs e) => this.PlaySoundFromQueue());

            this.isLooping = false;
            this.currentQueuePosition = 0;
            this.queue = new List<Sound>();
        }

        public static QueuePlaybackService Instance()
        {
            if (queuePlaybackService == null)
            {
                queuePlaybackService = new QueuePlaybackService();
            }

            return queuePlaybackService;
        }
        
        public SoundPlaybackService SoundPlaybackService { get; set; }

        public bool IsLooping()
        {
            return this.isLooping;
        }

        public void SetLooping(bool loop)
        {
            this.isLooping = loop;
            this.currentQueuePosition = 0;
        }
        
        public IEnumerable<Sound> GetQueue()
        {
            return this.queue;
        }

        public void AddSound(Sound sound)
        {
            this.queue.Add(sound);

            var isNoSoundPlaying = this.SoundPlaybackService.CurrentSoundOrDefault() == default(PlayableSound);
            if (isNoSoundPlaying)
            {
                this.PlaySoundFromQueue();
                return;
            }
        }

        public void RemoveSoundByIndex(int index)
        {
            if (this.queue.Count <= index)
            {
                return;
            }

            if (this.currentQueuePosition - 1 >= index && this.isLooping)
            {
                this.currentQueuePosition--;
            }

            this.queue.RemoveAt(index);
        }

        public void PlaySoundFromQueue()
        {
            if (this.isLooping)
            {
                this.PlayNextSoundFromQueue();
            }
            else
            {
                this.PlayFirstSoundFromQueue();
            }
        }

        private void PlayFirstSoundFromQueue()
        {
            if (this.queue.Count == 0)
            {
                this.SoundPlaybackService.PlaySoundOrStopIfNull(null);
                return;
            }

            this.SoundPlaybackService.PlaySoundOrStopIfNull(this.queue.First());
            this.queue.RemoveAt(0);
        }

        private void PlayNextSoundFromQueue()
        {
            if (this.currentQueuePosition >= this.queue.Count)
            {
                this.currentQueuePosition = 0;

                if (this.queue.Count == 0)
                {
                    this.SoundPlaybackService.PlaySoundOrStopIfNull(null);
                    return;
                }
            }

            this.SoundPlaybackService.PlaySoundOrStopIfNull(this.queue[this.currentQueuePosition]);
            this.currentQueuePosition++;
        }
    }
}
