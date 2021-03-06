// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../../_references.ts" />

module App.Controllers {
    'use strict';

    export class MainController extends Controller {
        
        // $inject: Avoid injection errors caused by minification
        static $inject = ['$scope', '$location', 'ToastService', 'IdentityService'];

        // Injected dependencies
        constructor(
            public $scope: Scopes.MainScope,
            private $location: ng.ILocationService,
            private ToastService: Services.ToastService,
            private IdentityService: Services.IdentityService
        ) {
            // Invoke parent constructor
            super($scope);
        }

        public Init() {
            // Called on page load
            this.$scope.CompanyCount = Math.round(Math.random() * 10000);
            this.$scope.ProductCount = Math.round(Math.random() * 10000);
            this.$scope.OrderCount = Math.round(Math.random() * 10000);
        }

        public GoTo(url: string) {
            this.$location.url(url);
        }
    }
} 
