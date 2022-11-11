namespace soundbox.views.index {
    interface IScope extends ng.IScope {
        userCount: number;
    }

    function ViewModel($scope: IScope, signalrService: soundbox.services.ISignalrService) {
        $scope.userCount = 0;

        signalrService.registerUpdateUsersOnlineCountHandler(count => $scope.userCount = count);
    }

    angular.module('soundbox')
        .controller('soundbox.views.index', ViewModel);
}