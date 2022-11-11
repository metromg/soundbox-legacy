/// <reference path="signalrService.ts" />
/// <reference path="soundViewService.ts" />
/// <reference path="playbackViewService.ts" />
/// <reference path="audioPlayerService.ts" />

angular.module('soundbox.services', [
    'soundbox.services.signalr',
    'soundbox.services.sound',
    'soundbox.services.playback',
    'soundbox.services.audioplayer'
]);