namespace Soundbox.Reloaded.Ui.Presentation.ViewServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Soundbox.Reloaded.Core.Domain.Sounds.Model;
    using Soundbox.Reloaded.Infrastructure.DbAccess;
    using Soundbox.Reloaded.Ui.Presentation.Dto.Sounds;

    public class SoundViewService : IDisposable
    {
        private readonly UnitOfWork unitOfWork;

        public SoundViewService()
        {
            this.unitOfWork = new UnitOfWork();
        }

        public IEnumerable<string> AddNewCategoryWithSound(SoundCategoryDto soundCategoryDto)
        {
            var soundCategory = Mapper.Map<SoundCategoryDto, SoundCategory>(soundCategoryDto);

            var errors = new List<string>();
            foreach (var sound in soundCategory.Sounds)
            {
                errors.AddRange(sound.Validate());
            }

            errors.AddRange(soundCategory.Validate());

            if (errors.Any())
            {
                return errors;
            }

            this.unitOfWork.SoundCategoryRepository.Add(soundCategory);
            this.unitOfWork.Commit();

            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> AddNewSoundToCategory(SoundDto soundDto)
        {
            var sound = Mapper.Map<SoundDto, Sound>(soundDto);

            var errors = sound.Validate();

            if (errors.Any())
            {
                return errors;
            }

            this.unitOfWork.SoundRepository.Add(sound);
            this.unitOfWork.Commit();

            return Enumerable.Empty<string>();
        }

        public IEnumerable<SoundCategoryDto> GetAllCategoriesWithSounds()
        {
            return this.unitOfWork.SoundCategoryRepository.GetAll()
                .Select(Mapper.Map<SoundCategory, SoundCategoryDto>);
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }
    }
}
