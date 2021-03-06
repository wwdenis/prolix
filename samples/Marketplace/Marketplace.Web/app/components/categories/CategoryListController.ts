// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../../_references.ts" />

module App.Controllers {
    'use strict';

    export class CategoryListController extends PagedController<Scopes.CategoryListScope> {
        
        // $inject: Avoid injection errors caused by minification
        static $inject = ['$scope', '$location', 'DialogService', 'ToastService', 'CategoryService'];

        // Injected dependencies
        constructor(
            public $scope: Scopes.CategoryListScope,
            private $location: ng.ILocationService,
            private DialogService: Services.DialogService,
            private ToastService: Services.ToastService,
            private CategoryService: Services.CategoryService
        ) {
            // Invoke parent constructor
            super($scope);
        }

        public Init() {
            // Api list criteria
            this.$scope.Query = new Queries.CategoryQuery();

            // List all
            this.Search();
        }

        public Add() {
            this.$location.url("/categories/add");
        }

        public Search() {
            var scope = this.$scope;
            var promise = this.CategoryService.Search(scope.Query);

            this.Wait();
            
            promise
                .then((result: Queries.PagedList<Models.Category>) => {
                    scope.Items = result.Items;
                    scope.PageCount = result.PageCount;
                    scope.RecordCount = result.RecordCount;

                    if (result.RecordCount == 0) {
                        this.ToastService.NoRecords();
                    }
                })
                .catch((error: Models.ErrorResult) => {
                    scope.Rule = error.Rule;
                    this.ToastService.Warning(error.Text);
                })
                .finally(() => {
                    this.Done();
                });
        }

        public TryDelete(id: any) {
            var dialog = this.DialogService.Confirm("Do you want to delete this entry?");

            dialog.then((result: App.Models.DialogResult) => {
                if (result == App.Models.DialogResult.Yes) {
                    this.Delete(id);
                }
            });
        }

        public Delete(id: any) {
            var scope = this.$scope;
            var promise = this.CategoryService.Delete(id);

            this.Wait();

            promise
                .then((result: boolean) => {
                    this.ToastService.Deleted();
                    this.Search();
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
