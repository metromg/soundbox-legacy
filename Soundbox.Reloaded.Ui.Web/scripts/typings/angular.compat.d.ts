declare module angular {
    export module auto {
        interface IInjectorService {
            get(name: string, caller?: string): any;
            instantiate(typeConstructor: Function, locals?: any): any;
        }
    }

    interface IFilterFunc {
        // TODO: validate
        <T>(value: T, ...params: any[]): T;
    }
}

declare module angular.animate {
    interface IAnimateService {
        on(event: string, container: Element | JQuery, callback: (element: Element, phase: string) => void): void;
        off(event: string, container?: Element | JQuery, callback?: Function): void;
    }
}