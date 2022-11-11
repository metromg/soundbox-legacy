module soundbox.domain.sounds {
    export class SoundDto extends EntityBaseDto {
        public CategoryId: string;
        public CategoryName: string;
        public Title: string;
        public ArtistName: string;
        public SoundFileName: string;
        public ImageFileName: string;

        public imageFilePathOrDefault(): string {
            return this.ImageFileName == null || this.ImageFileName == ""
                 ? '/assets/default-sound.png'
                 : '/ImageFiles/' + this.ImageFileName;
        }
    }
}