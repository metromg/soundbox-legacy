module soundbox.ui.directives.soundListItem {
    import SoundDto = soundbox.domain.sounds.SoundDto;

    interface IScope extends ng.IScope {
        sound: SoundDto;

        isInQueue: boolean;
        isPlaying: boolean;
        onDeleteClick(sound: SoundDto): void;
    }

    function Directive(): ng.IDirective {
        let directive = <ng.IDirective>{};

        directive.restrict = 'E';
        directive.replace = true;
        directive.templateUrl = 'directives/sound-list-item';
        directive.scope = { sound: '=', isInQueue: '=', isPlaying: '=', onDeleteClick: '=' };

        directive.link = function (scope: IScope, element: JQuery, attrs: ng.IAttributes) {
            scope.isInQueue = scope.isInQueue || false;
            scope.isPlaying = scope.isPlaying || false;
            scope.onDeleteClick = scope.onDeleteClick || null;
        }

        return directive;
    }

    angular.module('soundbox.ui.directives.sound-list-item', [])
        .directive('soundListItem', Directive);
}