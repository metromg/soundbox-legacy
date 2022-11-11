module soundbox.services {
    import SoundDto = soundbox.domain.sounds.SoundDto;
    import PlayableSoundDto = soundbox.domain.sounds.PlayableSoundDto;

    export interface IPlaybackViewService {
        getQueue(): ng.IHttpPromise<SoundDto[]>;
        getCurrentSound(): ng.IHttpPromise<PlayableSoundDto>;
        isLooping(): ng.IHttpPromise<boolean>;
        setLooping(loop: boolean): ng.IHttpPromise<{}>;
        playSoundFromQueue(): ng.IPromise<{}>;
        playPause(): ng.IPromise<{}>;
        seek(seconds: number): ng.IPromise<{}>;
        addToQueue(sound: SoundDto): ng.IPromise<{}>;
        removeFromQueue(index: number): ng.IPromise<{}>;
    }

    export class PlaybackViewService implements IPlaybackViewService {
        private http: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.http = $http;
        }

        public getQueue() {
            return this.http.get<SoundDto[]>('/api/sound/getQueue');
        }

        public getCurrentSound() {
            return this.http.get<PlayableSoundDto>('/api/sound/getCurrentSound');
        }

        public isLooping() {
            return this.http.get<boolean>('/api/sound/isLooping');
        }

        public setLooping(loop: boolean) {
            return this.http.post('/api/sound/setLooping?loop=' + loop, {});
        }

        public playSoundFromQueue() {
            return this.http.post('/api/sound/playSoundFromQueue', {});
        }

        public playPause() {
            return this.http.post('/api/sound/playPause', {});
        }

        public seek(seconds: number) {
            return this.http.post('/api/sound/seek?seconds=' + seconds, {});
        }

        public addToQueue(sound: SoundDto) {
            return this.http.post('/api/sound/addToQueue?soundId=' + sound.Id, {});
        }

        public removeFromQueue(index: number) {
            return this.http.post('/api/sound/removeFromQueue?index=' + index, {});
        }
    }

    angular.module('soundbox.services.playback', [])
        .service('playbackViewService', PlaybackViewService);
}