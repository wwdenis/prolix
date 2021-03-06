// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Services {
    "use strict";

    interface DialogScope extends ng.ui.bootstrap.IModalScope {
        Options: App.Models.DialogOptions;
    }

    export class DialogService implements IService {

        static $inject = ['$uibModal', 'Application'];

        public Defaults: ng.ui.bootstrap.IModalSettings;
        public Options: App.Models.DialogOptions;

        constructor(private $uibModal: ng.ui.bootstrap.IModalService, private Application: IApplication) {

            this.Defaults = {
                backdrop: true,
                keyboard: true,
                templateUrl: "/app/components/shared/modal.html"
            };

            this.Options = {
                HeaderText: "Confirm",
                BodyText: "Proceed?",
                
                OkText: "OK",
                CloseText: "Cancel",

                OkResult: App.Models.DialogResult.Ok,
                CloseResult: App.Models.DialogResult.Cancel
            };
        }

        public Message(text: string, title?: string): ng.IPromise<App.Models.DialogResult> {
            if (Utils.IsEmpty(text))
                throw new Error("Message text cannot be null!");

            if (Utils.IsEmpty(title))
                title = "Alerta";

            var options: App.Models.DialogOptions = {
                HeaderText: title,
                BodyText: text
            };

            return this.Show(options);
        }

        public Confirm(text: string, title?: string): ng.IPromise<App.Models.DialogResult> {
            if (Utils.IsEmpty(text))
                text = "Proceed?";

            if (Utils.IsEmpty(title))
                title = "Confirm";

            var options : App.Models.DialogOptions = {
                CloseText: "No",
                OkText: "Yes",
                HeaderText: title,
                BodyText: text,
                OkResult: App.Models.DialogResult.Yes,
                CloseResult: App.Models.DialogResult.No
            };

            return this.Show(options);
        }

        public Show(options?: App.Models.DialogOptions, defaults?: ng.ui.bootstrap.IModalSettings): ng.IPromise<App.Models.DialogResult> {
            if (!defaults)
                defaults = {};

            var controllerAlias = this.Application.ControllerAlias;

            defaults.backdrop = "static";

            //Create temp objects to work with since we"re in a singleton service
            var modalDefaults: ng.ui.bootstrap.IModalSettings = {};
            var modalOptions: App.Models.DialogOptions = {};

            //Map angular-ui modal custom defaults to modal defaults defined in service
            angular.extend(modalDefaults, this.Defaults, defaults);

            //Map modal.html $scope custom properties to defaults defined in service
            angular.extend(modalOptions, this.Options, options);

            modalOptions.OkEnabled = !Utils.IsEmpty(modalOptions.OkText);
            modalOptions.CloseEnabled = !Utils.IsEmpty(modalOptions.CloseText);

            if (!modalDefaults.controller) {
                
                modalDefaults.controllerAs = controllerAlias;
                modalDefaults.controller = function ($scope: DialogScope, $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {
                    $scope.Options = modalOptions;
                    this.Ok = function () {
                        $uibModalInstance.close(modalOptions.OkResult);
                    };
                    this.Close = function () {
                        $uibModalInstance.close(modalOptions.CloseResult);
                    };
                }
            }

            var instance = this.$uibModal.open(modalDefaults)
            var promise = instance.result;

            return promise;
        }
    }
}
