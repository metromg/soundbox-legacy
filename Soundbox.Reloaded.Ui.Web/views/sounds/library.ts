module soundbox.views.sounds.library {
    import SoundCategoryDto = soundbox.domain.sounds.SoundCategoryDto;
    import SoundDto = soundbox.domain.sounds.SoundDto;

    interface IScope extends ng.IScope {
        keyword: string;
        library: SoundCategoryDto[];
        allSounds: SoundDto[];

        queue: SoundDto[];
        isSoundInQueue(sound: SoundDto): boolean;

        addToQueue(sound: SoundDto): void;
        
        uploadSound(category: SoundCategoryDto): void;
    }

    function ViewModel($scope: IScope, $uibModal: ng.ui.bootstrap.IModalService, soundViewService: soundbox.services.ISoundViewService, playbackViewService: soundbox.services.IPlaybackViewService, signalrService: soundbox.services.ISignalrService) {
        $scope.keyword = "";
        
        const setLibrary = (library: SoundCategoryDto[]) => {
            var categories: SoundCategoryDto[] = library.map(c => angular.merge(new SoundCategoryDto(), c));
            categories.forEach(c => c.Sounds = c.Sounds.map(s => angular.merge(new SoundDto(), s)));

            $scope.library = categories;
            $scope.allSounds = categories.reduce((accumulator: SoundDto[], current: SoundCategoryDto) => accumulator.concat(current.Sounds), []);
        }

        const setQueue = (queue: SoundDto[]) => {
            $scope.queue = queue;
        }

        setLibrary([]);
        setQueue([]);

        soundViewService.getAllCategoriesWithSounds().success(setLibrary);
        signalrService.registerRefreshAvailableSoundsHandler(setLibrary);

        playbackViewService.getQueue().success(setQueue);
        signalrService.registerRefreshQueueHandler(setQueue);

        $scope.isSoundInQueue = sound => {
            var matchingSoundsInQueue = $scope.queue.filter(soundInQueue => soundInQueue.Id == sound.Id);
            return matchingSoundsInQueue.length > 0;
        }

        $scope.addToQueue = sound =>
            playbackViewService.addToQueue(sound);

        $scope.uploadSound = category => {
            const modalInstance = $uibModal.open({
                templateUrl: 'upload-editor-template.html',
                controller: EditorViewModel,
                backdrop: 'static',
                resolve: {
                    category: category
                }
            });
        }
    }

    enum EditorMode {
        NewCategory = 0,
        ExistingCategory = 1
    }

    interface IEditorScope extends ng.IScope {
        form: ng.IFormController;

        editorMode: EditorMode;

        category: SoundCategoryDto;
        sound: SoundDto;

        soundFile: File;
        imageFile: File;

        invalidImageFiles: File[];

        commit(): void;
        cancel(): void;

        uploadInProgress: boolean;
        uploadProgress: number;
    }

    function EditorViewModel($scope: IEditorScope, $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance, soundViewService: soundbox.services.ISoundViewService, category: SoundCategoryDto) {
        $scope.category = angular.copy(category);
        $scope.editorMode = EditorMode.ExistingCategory;

        $scope.invalidImageFiles = [];

        $scope.uploadInProgress = false;
        $scope.uploadProgress = 0;

        if ($scope.category == null) {
            $scope.category = new SoundCategoryDto();
            $scope.editorMode = EditorMode.NewCategory;
        }

        $scope.sound = new SoundDto();

        $scope.commit = () => {
            if ($scope.uploadInProgress || $scope.form.$invalid) {
                return;
            }

            $scope.uploadInProgress = true;

            if ($scope.editorMode == EditorMode.NewCategory) {
                uploadSoundToNewCategory();
            }

            if ($scope.editorMode == EditorMode.ExistingCategory) {
                uploadSoundToExistingCategory();
            }
        }
        
        $scope.cancel = () => {
            if ($scope.uploadInProgress) {
                return;
            }

            $uibModalInstance.dismiss('cancel');
        }

        function uploadSoundToNewCategory() {
            var category = angular.copy($scope.category);
            var sound = angular.copy($scope.sound);
            category.Sounds = [];
            category.Sounds.push(sound);

            soundViewService.addNewSoundWithDescription(category, $scope.soundFile, $scope.imageFile, refreshUploadProgress, finalizeCommit, error);
        }

        function uploadSoundToExistingCategory() {
            var sound = angular.copy($scope.sound);
            var categoryId = $scope.category.Id;
            sound.CategoryId = categoryId;

            soundViewService.addNewSoundWithDescription(sound, $scope.soundFile, $scope.imageFile, refreshUploadProgress, finalizeCommit, error);
        }

        function refreshUploadProgress(evt: ProgressEvent) {
            $scope.uploadProgress = (evt.loaded / evt.total) * 100;
        }

        function finalizeCommit(result: any) {
            $scope.uploadInProgress = false;
            $uibModalInstance.close();
        }

        function error(result: any) {
            $scope.uploadInProgress = false;
            $scope.uploadProgress = 0;

            alert("There was an error while uploading the file");
        }
    }

    export const view: any = {
        templateUrl: 'views/sounds/library',
        controller: ViewModel
    }
}