module soundbox.views.sounds.queue {
    import SoundDto = soundbox.domain.sounds.SoundDto;
    import PlayableSoundDto = soundbox.domain.sounds.PlayableSoundDto;

    interface IScope extends ng.IScope {
        queue: SoundDto[];
        isLooping: boolean;

        currentSound: PlayableSoundDto;
        isSoundPlaying(sound: SoundDto): boolean;

        removeFromQueue(sound: SoundDto): void;
        setLooping(): void;
    }

    function ViewModel($scope: IScope, playbackViewService: soundbox.services.IPlaybackViewService, signalrService: soundbox.services.ISignalrService) {
        const setQueue = (queue: SoundDto[]) =>
            $scope.queue = queue.map(s => angular.merge(new SoundDto(), s));

        const setIsLooping = (isLooping: boolean) =>
            $scope.isLooping = isLooping;

        const setCurrentSound = (currentSound: PlayableSoundDto) =>
            $scope.currentSound = currentSound;

        setQueue([]);
        setIsLooping(false);
        setCurrentSound(null);

        playbackViewService.getQueue().success(setQueue);
        playbackViewService.isLooping().success(setIsLooping);
        playbackViewService.getCurrentSound().success(setCurrentSound);
        signalrService.registerRefreshQueueHandler(setQueue);
        signalrService.registerRefreshIsLoopingHandler(setIsLooping);
        signalrService.registerRefreshCurrentSoundHandler(setCurrentSound);

        $scope.removeFromQueue = sound =>
            playbackViewService.removeFromQueue($scope.queue.indexOf(sound));

        $scope.setLooping = () =>
            playbackViewService.setLooping(!$scope.isLooping);

        $scope.isSoundPlaying = sound => {
            if ($scope.currentSound == null) {
                return false;
            }

            return $scope.currentSound.Sound.Id == sound.Id;
        }
    }

    export const view: any = {
        templateUrl: 'views/sounds/queue',
        controller: ViewModel
    }
}