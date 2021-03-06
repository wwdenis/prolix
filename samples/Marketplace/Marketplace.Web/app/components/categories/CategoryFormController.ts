// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../../_references.ts" />

module App.Controllers {
    'use strict';

    export class CategoryFormController extends Controller {
        
        // $inject: Avoid injection errors caused by minification
        static $inject = ['$scope', '$location', '$stateParams', 'ToastService', 'CategoryService'];

        // Injected dependencies
        constructor(
            public $scope: Scopes.CategoryFormScope,
            private $location: ng.ILocationService,
            private $stateParams: App.StateParameters,
            private ToastService: Services.ToastService,
            private CategoryService: Services.CategoryService
        ) {
            // Invoke parent constructor
            super($scope);

            // Gets the route parameter
            this.$scope.Id = $stateParams.Id;
        }

        public Init() {
            // Scope initialization
            this.$scope.Form = new Models.Category();

            if (!this.IsNew()) {
                // Load from api
                this.Load();
            }
        }

        public NameRemaining() {
            if (Utils.IsUndefined(this.$scope.Form))
                return 100;

            return super.Remainder(100, this.$scope.Form.Name);
        }

        public IsNew() {
            return Utils.IsUndefined(this.$scope.Id) || this.$scope.Id <= 0;
        }

        public Back() {
            this.$location.url("/categories");
        }

        public Save() {
            
            var scope = this.$scope;
            var location = this.$location;

            this.Wait();

            // Sends out data for insert/update
            var promise = this.CategoryService.Save(scope.Form);

            promise
                .then((result: any) => {
                    location.url('/categories');
                })
                .catch((error: Models.ErrorResult) => {
                    scope.Rule = error.Rule;
                    this.ToastService.Warning(error.Text);
                })
                .finally(() => {
                    this.Done();
                });
        }

        public Load() {
            // Load from api
            
            var scope = this.$scope;
            var promise = this.CategoryService.Get(scope.Id);

            this.Wait();

            promise
                .then((result: Models.Category) => {
                    scope.Form = result;
                })
                .catch((error: Models.ErrorResult) => {
                    if (error.Rule != null)
                        scope.Rule = error.Rule;

                    this.ToastService.Warning(error.Text);
                })
                .finally(() => {
                    this.Done();
                });
        }
    }
} 
