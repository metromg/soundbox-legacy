namespace Soundbox.Reloaded.Ui.Presentation.Dto
{
    using AutoMapper;

    using Soundbox.Reloaded.Core.Domain;
    using Soundbox.Reloaded.Core.Domain.Sounds.Model;
    using Soundbox.Reloaded.Core.Domain.Sounds.Objects;
    using Soundbox.Reloaded.Ui.Presentation.Dto.Sounds;

    public class MappingConfiguration
    {
        public static void Configure()
        {
            ConfigureModelToDto();
            ConfigureDtoToModel();

            Mapper.AssertConfigurationIsValid();
        }

        private static void ConfigureModelToDto()
        {
            Mapper.CreateMap<SoundCategory, SoundCategoryDto>();
            Mapper.CreateMap<Sound, SoundDto>()
                .ForMember(m => m.CategoryName, opt => opt.MapFrom(s => s.Category.Name));

            Mapper.CreateMap<PlayableSound, PlayableSoundDto>();
        }

        private static void ConfigureDtoToModel()
        {
            CreateDtoToEntityMap<SoundCategoryDto, SoundCategory>();
            CreateDtoToEntityMap<SoundDto, Sound>()
                .ForMember(m => m.Category, opt => opt.Ignore());

            Mapper.CreateMap<PlayableSoundDto, PlayableSound>();
        }

        private static IMappingExpression<TSource, TDestination> CreateDtoToEntityMap<TSource, TDestination>()
            where TSource : EntityBaseDto
            where TDestination : EntityBase
        {
            var map = Mapper.CreateMap<TSource, TDestination>()
                .ForMember(m => m.Id, opt => opt.Ignore());

            return map;
        }
    }
}
