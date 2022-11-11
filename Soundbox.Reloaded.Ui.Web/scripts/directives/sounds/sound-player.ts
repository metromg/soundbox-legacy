module soundbox.ui.directives.soundPlayer {
    import PlayableSoundDto = soundbox.domain.sounds.PlayableSoundDto;

    interface IScope extends ng.IScope {
        sound: PlayableSoundDto;

        playPause(): void;
        playFromQueue(): void;
        mute(): void;

        seekValue: number;

        currentTimeInPercent: number;
        currentTimeToDate(): Date;

        initialVolume: number;
        volume: number;
        muted: boolean;
    }

    function Directive(audioPlayerService: soundbox.services.IAudioPlayerService, playbackViewService: soundbox.services.IPlaybackViewService, signalrService: soundbox.services.ISignalrService): ng.IDirective {
        let directive = <ng.IDirective>{};

        directive.restrict = 'E';
        directive.replace = true;
        directive.templateUrl = 'directives/sound-player';
        directive.scope = { sound: '=' };

        directive.link = function (scope: IScope, element: JQuery, attrs: ng.IAttributes) {
            scope.initialVolume = audioPlayerService.getVolume();
            scope.muted = audioPlayerService.getMuted();

            scope.$watch(s => scope.sound, sound => {
                if (sound == null) {
                    audioPlayerService.play(null, 0, false);
                    return;
                }

                audioPlayerService.play(sound.Sound.SoundFileName, sound.CurrentTime, sound.IsPlaying);
            });

            scope.$watch(s => scope.seekValue, value => {
                if (value == null) {
                    return;
                }

                var seekTime = percentageToSeconds(value);
                playbackViewService.seek(seekTime);
            });

            scope.$watch(s => scope.volume, volume => {
                if (volume == null) {
                    return;
                }

                audioPlayerService.setVolume(volume);
            });
            
            audioPlayerService.registerTimeUpdateHandler(audio => {
                if (scope.sound == null) {
                    return;
                }

                scope.$apply(() => scope.sound.CurrentTime = audio.currentTime);
            });

            signalrService.registerPauseHandler(() => {
                scope.sound.IsPlaying = false;
                audioPlayerService.pause();
            });

            scope.playPause = () => {
                playbackViewService.playPause();
            }

            scope.playFromQueue = () => {
                playbackViewService.playSoundFromQueue();
            }

            scope.mute = () => {
                audioPlayerService.setMuted(!scope.muted);
                scope.muted = !scope.muted;
            }

            scope.currentTimeToDate = () => {
                let tempSound = new PlayableSoundDto();
                tempSound.CurrentTime = percentageToSeconds(scope.currentTimeInPercent);
                return tempSound.currentTimeToDate();
            };

            function percentageToSeconds(percentage: number) {
                if (scope.sound == null) {
                    return 0;
                }

                return Math.round((percentage / 100) * scope.sound.Duration);
            }
        }

        return directive;
    }

    angular.module('soundbox.ui.directives.sound-player', [])
        .directive('soundPlayer', Directive);
}