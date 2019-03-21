// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../../_references.ts" />

module App.Controllers {
    'use strict';

    export class RegisterController extends Controller {
        
        // $inject: Avoid injection errors caused by minification
        static $inject = ['$scope', '$location', 'ToastService', 'IdentityService'];

        // Injected dependencies
        constructor(
            public $scope: Scopes.RegisterScope,
            private $location: ng.ILocationService,
            private ToastService: Services.ToastService,
            private IdentityService: Services.IdentityService
        ) {
            // Invoke parent constructor
            super($scope);
        }

        public Init() {
            // Called on page load
            // this.IdentityService.Logout();

            var model = new Models.Register();
            this.$scope.Input = model;
        }

        public Back() {
            this.$location.url("/");
        }

        public Confirm() {
            var scope = this.$scope;
            var location = this.$location;

            this.Wait();

            var promise = this.IdentityService.Register(scope.Input);

            promise
                .then((data: Models.Access) => {
                    this.IdentityService.CreateContext(data, false);
                    this.$location.path("/");
                })
                .catch((error: Models.ErrorResult) => {
                    this.IdentityService.ClearContext();
                    this.$scope.Rule = error.Rule;
                    this.ToastService.Warning(error.Text);
                })
                .finally(() => {
                    this.Done();
                });
        }

    }
} 
