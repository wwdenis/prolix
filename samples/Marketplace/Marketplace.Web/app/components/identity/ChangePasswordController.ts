// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../../_references.ts" />

module App.Controllers {
    'use strict';

    export class ChangePasswordController extends Controller {
        
        // $inject: Avoid injection errors caused by minification
        static $inject = ['$scope', '$location', 'ToastService', 'IdentityService'];

        // Injected dependencies
        constructor(
            public $scope: Scopes.ChangePasswordScope,
            private $location: ng.ILocationService,
            private ToastService: Services.ToastService,
            private IdentityService: Services.IdentityService
        ) {
            // Invoke parent constructor
            super($scope);
        }

        public Init() {
            // Called on page load
            var model = new Models.PasswordChange();
            this.$scope.Input = model;
        }

        public Back() {
            this.$location.url("/");
        }

        public Confirm() {
            var scope = this.$scope;
            var location = this.$location;

            this.Wait();

            var promise = this.IdentityService.ChangePassword(scope.Input);

            promise
                .then((result: any) => {
                    location.url('/');
                })
                .catch((error: Models.ErrorResult) => {
                    scope.Rule = error.Rule;
                    this.ToastService.Warning(error.Text);
                })
                .finally(() => {
                    this.Done();
                });
        }

    }
} 
