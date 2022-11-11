module soundbox.services {
    import SoundCategoryDto = soundbox.domain.sounds.SoundCategoryDto;

    export interface ISoundViewService {
        getAllCategoriesWithSounds(): ng.IHttpPromise<SoundCategoryDto[]>;
        addNewSoundWithDescription<T>(description: T, soundFile: File, imageFile: File, onProgress: (evt: ProgressEvent) => any, onSuccess: (result: any) => any, onError: (result: any) => any): void;
    }

    export class SoundViewService implements ISoundViewService {
        private http: ng.IHttpService;
        private upload: angular.angularFileUpload.IUploadService;

        constructor($http: ng.IHttpService, Upload: angular.angularFileUpload.IUploadService) {
            this.http = $http;
            this.upload = Upload;
        }

        public getAllCategoriesWithSounds() {
            return this.http.get<SoundCategoryDto[]>('/api/sound/getAllCategoriesWithSounds');
        }

        public addNewSoundWithDescription<T>(description: T, soundFile: File, imageFile: File, onProgress: (evt: ProgressEvent) => any, onSuccess: (result: any) => any, onError: (result: any) => any) {
            let currentUpload = this.upload.upload({
                url: description instanceof SoundCategoryDto ? '/api/sound/addNewCategoryWithSound' : '/api/sound/addNewSoundToCategory',
                method: "POST",
                data: { info: angular.toJson(description) },
                file: [soundFile, imageFile]
            })
            .progress(onProgress)
            .success(onSuccess)
            .error(onError);
        }
    }

    angular.module('soundbox.services.sound', [])
        .service('soundViewService', SoundViewService);
}