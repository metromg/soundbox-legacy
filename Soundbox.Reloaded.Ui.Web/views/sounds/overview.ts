module soundbox.views.sounds.overview {
    import PlayableSoundDto = soundbox.domain.sounds.PlayableSoundDto;

    interface IScope extends ng.IScope {
        currentSound: PlayableSoundDto;
    }

    function ViewModel($scope: IScope, playbackViewService: soundbox.services.IPlaybackViewService, signalrService: soundbox.services.ISignalrService) {
        const setCurrentSound = (sound: PlayableSoundDto) =>
            $scope.currentSound = sound == null ? null : angular.merge(new PlayableSoundDto(), sound);

        setCurrentSound(null);

        playbackViewService.getCurrentSound().success(setCurrentSound);
        signalrService.registerRefreshCurrentSoundHandler(setCurrentSound);
    }

    export const route: ng.ui.IState = {
        name: 'overview',
        url: '/overview',
        views: {
            '@': {
                templateUrl: 'views/sounds/overview',
                controller: ViewModel
            },
            'queue@overview': soundbox.views.sounds.queue.view,
            'library@overview': soundbox.views.sounds.library.view
        }
    }
}