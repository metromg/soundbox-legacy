angular.module('soundbox.routing', ['ui.router'])
    .config(($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider, $uiViewScrollProvider: any, $httpProvider: ng.IHttpProvider) => {
        $uiViewScrollProvider.useAnchorScroll();

        $urlRouterProvider.otherwise(<string>soundbox.views.sounds.overview.route.url);
        $stateProvider
            .state(soundbox.views.sounds.overview.route);
    });