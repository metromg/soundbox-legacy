angular.module('soundbox', ['soundbox.routing', 'soundbox.ui', 'soundbox.domain', 'soundbox.services', 'ui.bootstrap', 'ngFileUpload', 'ngAnimate', 'ngTouch'])
    .run(($rootScope: any, $rootElement: ng.IRootElementService, $state: any, $stateParams: any, remotingHostRootUri: string, $animate: ng.animate.IAnimateService, signalrService: soundbox.services.ISignalrService) => {
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;
        $rootScope.$ignore = () => false;
        $rootScope.$today = moment().startOf('day').toDate();
        $rootScope.$tomorrow = moment().add(1, 'day').startOf('day').toDate();
        $rootScope.$yesterday = moment().add(-1, 'day').startOf('day').toDate();

        signalrService.startConnection();
    });