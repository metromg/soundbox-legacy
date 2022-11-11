module soundbox.services {
    export interface IAudioPlayerService {
        play(filename: string, startTime: number, isPlaying: boolean): void;
        pause(): void;
        setVolume(volume: number): void;
        setMuted(muted: boolean): void;
        getVolume(): number;
        getMuted(): boolean;

        registerTimeUpdateHandler(handler: (audio: HTMLAudioElement) => any): void;
        registerPlayHandler(handler: (audio: HTMLAudioElement) => any): void;
        registerEndedHandler(handler: (audio: HTMLAudioElement) => any): void;
    }

    export class AudioPlayerService implements IAudioPlayerService {
        private audio: HTMLAudioElement;
        private volume: number;
        private muted: boolean;

        private ontimeupdateHandlers: ((audio: HTMLAudioElement) => any)[];
        private onplayHandlers: ((audio: HTMLAudioElement) => any)[];
        private onendedHandlers: ((audio: HTMLAudioElement) => any)[];

        constructor() {
            this.audio = null;

            this.volume = Number(window.localStorage.getItem('soundbox-volume')) || 100;
            this.muted = false;

            this.ontimeupdateHandlers = [];
            this.onplayHandlers = [];
            this.onendedHandlers = [];
        }

        public play(filename: string, startTime: number, isPlaying: boolean) {
            let canPlaySound = true;
            
            if (filename == null) {
                if (this.audio != null) {
                    this.audio.pause();
                }

                return;
            }

            if (this.audio != null) {
                this.audio.pause();
            }

            this.audio = new Audio("/api/sound/getSoundFile?filename=" + encodeURIComponent(filename));

            this.audio.ontimeupdate = () => this.ontimeupdateHandlers.forEach(handler => handler(this.audio));

            this.audio.oncanplay = () => {
                if (canPlaySound) {
                    setTimeout(() => {
                        this.audio.play();
                        this.audio.currentTime = startTime;
                        this.audio.volume = this.volume / 100;
                        this.audio.muted = this.muted;

                        if (!isPlaying) {
                            this.audio.pause();
                        }

                        canPlaySound = false;

                        this.onplayHandlers.forEach(handler => handler(this.audio));
                    }, 100);
                }
            }

            this.audio.onended = () => this.onendedHandlers.forEach(handler => handler(this.audio));
        }

        public pause() {
            this.audio.pause();
        }

        public setVolume(volume: number) {
            this.volume = volume;
            window.localStorage.setItem('soundbox-volume', volume.toString());

            if (this.audio != null) {
                this.audio.volume = volume / 100;
            }
        }

        public setMuted(muted: boolean) {
            this.muted = muted;

            if (this.audio != null) {
                this.audio.muted = muted;
            }
        }

        public getVolume() {
            return this.volume;
        }

        public getMuted() {
            return this.muted;
        }

        public registerTimeUpdateHandler(handler: (audio: HTMLAudioElement) => any) {
            this.ontimeupdateHandlers.push(handler);
        }

        public registerPlayHandler(handler: (audio: HTMLAudioElement) => any) {
            this.onplayHandlers.push(handler);
        }

        public registerEndedHandler(handler: (audio: HTMLAudioElement) => any) {
            this.onendedHandlers.push(handler);
        }
    }

    angular.module('soundbox.services.audioplayer', [])
        .service('audioPlayerService', AudioPlayerService);
}