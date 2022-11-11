module soundbox.domain.sounds {
    export class PlayableSoundDto {
        constructor() {
            this.Sound = new SoundDto();
        }

        public Sound: SoundDto;
        public IsPlaying: boolean;
        public CurrentTime: number;
        public Duration: number;

        public currentTimeToDate(): Date {
            return this.secondsToDate(this.CurrentTime);
        }

        public durationToDate(): Date {
            return this.secondsToDate(this.Duration);
        }

        public currentProgressInPercent(): number {
            return (this.CurrentTime / this.Duration) * 100;
        }

        private secondsToDate(seconds: number): Date {
            var date = new Date(1970, 0, 1);
            date.setSeconds(seconds);
            return date;
        }
    }
}