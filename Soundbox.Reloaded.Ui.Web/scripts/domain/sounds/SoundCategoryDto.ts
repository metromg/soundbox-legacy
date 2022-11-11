module soundbox.domain.sounds {
    export class SoundCategoryDto extends EntityBaseDto {
        public Name: string;
        public Sounds: SoundDto[];
    }
}