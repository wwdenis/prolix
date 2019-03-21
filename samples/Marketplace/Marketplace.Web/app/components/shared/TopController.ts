// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../../_references.ts" />

module App.Controllers {
    'use strict';

    export class TopController extends Controller {
        
        // $inject: Avoid injection errors caused by minification
        static $inject = ['$scope', '$location', 'ToastService', 'IdentityService'];

        // Injected dependencies
        constructor(
            public $scope: Scopes.TopScope,
            private $location: ng.ILocationService,
            private ToastService: Services.ToastService,
            private IdentityService: Services.IdentityService
        ) {
            // Invoke parent constructor
            super($scope);
        }

        public Init() {
            // Called on page load
        }

        public ChangePassword() {
            this.$location.url("/change-password");
        }

        public Logout() {
            var scope = this.$scope;
            var location = this.$location;

            //var promise = this.IdentityService.Logout();
            this.IdentityService.ClearContext();
            location.url('/login');
        }
    }
} 
