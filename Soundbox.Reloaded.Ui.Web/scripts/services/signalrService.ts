module soundbox.services {
    import SoundDto = soundbox.domain.sounds.SoundDto;
    import SoundCategoryDto = soundbox.domain.sounds.SoundCategoryDto;
    import PlayableSoundDto = soundbox.domain.sounds.PlayableSoundDto;

    export interface ISignalrService {
        startConnection(): void;

        registerRefreshCurrentSoundHandler(handler: (sound: PlayableSoundDto) => any): void;
        registerPauseHandler(hander: () => any): void;
        registerRefreshIsLoopingHandler(handler: (isLooping: boolean) => any): void;
        registerRefreshQueueHandler(handler: (queue: SoundDto[]) => any): void;
        registerRefreshAvailableSoundsHandler(handler: (categoriesWithSounds: SoundCategoryDto[]) => any): void;
        registerUpdateUsersOnlineCountHandler(handler: (count: number) => any): void;

        registerReconnectingHandler(handler: () => any): void;
        registerReconnectedHandler(handler: () => any): void;
        registerDisconnectedHandler(handler: () => any): void;
    }

    export class SignalrService implements ISignalrService {
        private hub = (<any>$.connection).soundHub;

        private refreshCurrentSoundHandlers: ((sound: PlayableSoundDto) => any)[];
        private pauseHandlers: (() => any)[];
        private refreshIsLoopingHandlers: ((isLooping: boolean) => any)[];
        private refreshQueueHandlers: ((queue: SoundDto[]) => any)[];
        private refreshAvailableSoundsHandlers: ((categoriesWithSounds: SoundCategoryDto[]) => any)[];
        private updateUsersOnlineCountHandlers: ((count: number) => any)[];

        private reconnectingHandlers: (() => any)[];
        private reconnectedHandlers: (() => any)[];
        private disconnectedHandlers: (() => any)[];

        constructor($rootScope: ng.IRootScopeService) {
            this.refreshCurrentSoundHandlers = [];
            this.pauseHandlers = [];
            this.refreshIsLoopingHandlers = [];
            this.refreshQueueHandlers = [];
            this.refreshAvailableSoundsHandlers = [];
            this.updateUsersOnlineCountHandlers = [];

            this.reconnectingHandlers = [];
            this.reconnectedHandlers = [];
            this.disconnectedHandlers = [];

            // Soundhub listeners
            this.hub.client.refreshCurrentSound = (sound: PlayableSoundDto) => {
                $rootScope.$apply(() => {
                    this.refreshCurrentSoundHandlers.forEach(handler => handler(sound));
                });
            };

            this.hub.client.pause = () => {
                $rootScope.$apply(() => {
                    this.pauseHandlers.forEach(handler => handler());
                });
            }

            this.hub.client.refreshIsLooping = (isLooping: boolean) => {
                $rootScope.$apply(() => {
                    this.refreshIsLoopingHandlers.forEach(handler => handler(isLooping));
                });
            };

            this.hub.client.refreshQueue = (queue: SoundDto[]) => {
                $rootScope.$apply(() => {
                    this.refreshQueueHandlers.forEach(handler => handler(queue));
                });
            };

            this.hub.client.refreshAvailableSounds = (categoriesWithSounds: SoundCategoryDto[]) => {
                $rootScope.$apply(() => {
                    this.refreshAvailableSoundsHandlers.forEach(handler => handler(categoriesWithSounds));
                });
            };

            this.hub.client.updateUsersOnlineCount = (count: number) => {
                $rootScope.$apply(() => {
                    this.updateUsersOnlineCountHandlers.forEach(handler => handler(count));
                });
            };
            
            // SignalR connection lifetime events
            var tryingToReconnect = false;

            $.connection.hub.reconnecting(() =>
                $rootScope.$apply(() => {
                    this.reconnectingHandlers.forEach(handler => handler());

                    tryingToReconnect = true;
                })
            );

            $.connection.hub.reconnected(() =>
                $rootScope.$apply(() => {
                    this.reconnectedHandlers.forEach(handler => handler());

                    tryingToReconnect = false;
                })
            );

            $.connection.hub.disconnected(() =>
                $rootScope.$apply(() => {
                    if (tryingToReconnect) {
                        this.disconnectedHandlers.forEach(handler => handler());
                    }
                })
            );
        }

        public startConnection(): void {
            $.connection.hub.start();
        }

        public registerRefreshCurrentSoundHandler(handler: (sound: PlayableSoundDto) => any) {
            this.refreshCurrentSoundHandlers.push(handler);
        }

        public registerPauseHandler(handler: () => any) {
            this.pauseHandlers.push(handler);
        }

        public registerRefreshIsLoopingHandler(handler: (isLooping: boolean) => any) {
            this.refreshIsLoopingHandlers.push(handler);
        }

        public registerRefreshQueueHandler(handler: (queue: SoundDto[]) => any) {
            this.refreshQueueHandlers.push(handler);
        }

        public registerRefreshAvailableSoundsHandler(handler: (categoriesWithSounds: SoundCategoryDto[]) => any) {
            this.refreshAvailableSoundsHandlers.push(handler);
        }

        public registerUpdateUsersOnlineCountHandler(handler: (count: number) => any) {
            this.updateUsersOnlineCountHandlers.push(handler);
        }

        public registerReconnectingHandler(handler: () => any) {
            this.reconnectingHandlers.push(handler);
        }

        public registerReconnectedHandler(handler: () => any) {
            this.reconnectedHandlers.push(handler);
        }

        public registerDisconnectedHandler(handler: () => any) {
            this.disconnectedHandlers.push(handler);
        }
    }

    angular.module('soundbox.services.signalr', [])
        .service('signalrService', SignalrService);
}