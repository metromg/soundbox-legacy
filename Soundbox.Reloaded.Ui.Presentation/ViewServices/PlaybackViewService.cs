namespace Soundbox.Reloaded.Ui.Presentation.ViewServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Soundbox.Reloaded.Core.Domain.Sounds.Model;
    using Soundbox.Reloaded.Core.Domain.Sounds.Objects;
    using Soundbox.Reloaded.Infrastructure.Services;
    using Soundbox.Reloaded.Infrastructure.DbAccess;
    using Soundbox.Reloaded.Ui.Presentation.Dto.Sounds;

    public class PlaybackViewService : IDisposable
    {
        private readonly UnitOfWork unitOfWork;
        private static bool soundEndsEventSubscribed = false;

        public PlaybackViewService(Action soundEndsAction)
        {
            this.unitOfWork = new UnitOfWork();

            if (soundEndsEventSubscribed == false)
            {
                QueuePlaybackService.Instance().SoundPlaybackService.SoundEnds += new SoundEndsEventHandler((object sender, EventArgs e) => soundEndsAction());
                soundEndsEventSubscribed = true;
            }
        }

        public IEnumerable<SoundDto> GetQueue()
        {
            return QueuePlaybackService.Instance().GetQueue()
                .Select(Mapper.Map<Sound, SoundDto>);
        }

        public PlayableSoundDto GetCurrentSound()
        {
            var sound = QueuePlaybackService.Instance().SoundPlaybackService.CurrentSoundOrDefault();
            return Mapper.Map<PlayableSound, PlayableSoundDto>(sound);
        }

        public bool IsLooping()
        {
            return QueuePlaybackService.Instance().IsLooping();
        }

        public void SetLooping(bool loop)
        {
            QueuePlaybackService.Instance().SetLooping(loop);
        }

        public void PlaySoundFromQueue()
        {
            QueuePlaybackService.Instance().PlaySoundFromQueue();
        }
        
        public bool PlayPause()
        {
            if (QueuePlaybackService.Instance().SoundPlaybackService.IsPlaying)
            {
                QueuePlaybackService.Instance().SoundPlaybackService.PauseSound();
                return false;
            }
            else
            {
                QueuePlaybackService.Instance().SoundPlaybackService.ContinueSound();
                return true;
            }
        }

        public void Seek(int seconds)
        {
            QueuePlaybackService.Instance().SoundPlaybackService.SeekSound(seconds);
        }

        public void AddToQueue(Guid soundId)
        {
            var sound = this.unitOfWork.SoundRepository.GetById(soundId);
            QueuePlaybackService.Instance().AddSound(sound);
        }

        public void RemoveFromQueue(int index)
        {
            QueuePlaybackService.Instance().RemoveSoundByIndex(index);
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }
    }
}
