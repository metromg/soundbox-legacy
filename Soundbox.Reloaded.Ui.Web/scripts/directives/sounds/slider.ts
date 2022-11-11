module soundbox.ui.directives.soundListItem {
    import SoundDto = soundbox.domain.sounds.SoundDto;

    interface IScope extends ng.IScope {
        programmaticValue: number;
        userValue: number;
        commitImmediately: boolean;

        sliderDisplayValue: number;

        mouseDown: boolean;
    }

    function Directive(): ng.IDirective {
        let directive = <ng.IDirective>{};

        directive.restrict = 'E';
        directive.replace = true;
        directive.templateUrl = 'directives/slider';
        directive.scope = { programmaticValue: '=', userValue: '=', commitImmediately: '=?', sliderDisplayValue: '=?' };

        directive.link = function (scope: IScope, element: JQuery, attrs: ng.IAttributes) {
            scope.commitImmediately = scope.commitImmediately || false;

            element.bind('mousemove', (e: any) => onMouseMove(e.clientX));
            element.bind('touchmove', (e: any) => onMouseMove(e.originalEvent.touches[0].clientX));
            element.children('div.slider').children('.range').mousedown(onMouseDown);
            element.children('div.slider').children('.range').bind('touchstart', onMouseDown);
            document.addEventListener('mouseup', onMouseUp);
            document.addEventListener('touchend', onMouseUp);

            scope.$watch(() => scope.programmaticValue, value => {
                if (scope.mouseDown) {
                    return;
                }

                scope.sliderDisplayValue = value;

                if (scope.commitImmediately) {
                    commit(value);
                }
            });

            function commit(value: number) {
                scope.userValue = value;
            }

            function onMouseDown() {
                scope.mouseDown = true;
            }

            function onMouseMove(clientX: number) {
                if (!scope.mouseDown) {
                    return;
                }
                
                const sliderWidth = element.width();
                const x = clientX - element.offset().left;

                let value = (100 / sliderWidth) * x;

                if (value < 0) {
                    value = 0;
                }

                if (value > 100) {
                    value = 100;
                }

                scope.sliderDisplayValue = value;

                if (scope.commitImmediately) {
                    commit(value);
                }

                scope.$apply();
            };

            function onMouseUp(e: MouseEvent) {
                if (!scope.mouseDown) {
                    return;
                }

                commit(scope.sliderDisplayValue);
                scope.mouseDown = false;
            }
        }

        return directive;
    }

    angular.module('soundbox.ui.directives.slider', [])
        .directive('slider', Directive);
}