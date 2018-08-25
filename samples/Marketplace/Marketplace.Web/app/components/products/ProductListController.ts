// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../../_references.ts" />

module App.Controllers {
    'use strict';

    export class ProductListController extends PagedController<Scopes.ProductListScope> {
        
        // $inject: Avoid injection errors caused by minification
        static $inject = ['$scope', '$location', 'DialogService', 'ToastService', 'CategoryService', 'DealerService', 'ProductService'];

        // Injected dependencies
        constructor(
            public $scope: Scopes.ProductListScope,
            private $location: ng.ILocationService,
            private DialogService: Services.DialogService,
            private ToastService: Services.ToastService,
            private CategoryService: Services.CategoryService,
            private DealerService: Services.DealerService,
            private ProductService: Services.ProductService
        ) {
            // Invoke parent constructor
            super($scope);
        }

        public Init() {
            // Api list criteria
            this.$scope.Query = new Queries.ProductQuery();
            this.$scope.Dealer = new Models.AutoCompleteConfig<Models.Dealer>();

            // Load lists
            this.LoadCategories();
        
            // List all
            this.Search();
        }

        public Add() {
            this.$location.url("/products/add");
        }

        public Search() {
            
            var scope = this.$scope;
            var promise = this.ProductService.Search(scope.Query);

            this.Wait();

            promise
                .then((result: Queries.PagedList<Models.Product>) => {
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
            
            var dialog = this.DialogService.Confirm("Do you want to delete this product?");

            dialog.then((result: App.Models.DialogResult) => {
                if (result == App.Models.DialogResult.Yes) {
                    this.Delete(id);
                }
            });
        }

        public Delete(id: any) {
            var scope = this.$scope;
            var promise = this.ProductService.Delete(id);

            promise
                .then((result: any) => {
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

        private LoadCategories() {
            var scope = this.$scope;
            var promise = this.CategoryService.All();
            
            this.Wait();

            promise
                .then((result: Models.Category[]) => {
                    scope.Categories = result;
                })
                .catch((error: Models.ErrorResult) => {
                    scope.Rule = error.Rule;
                    this.ToastService.Warning(error.Text);
                })
                .finally(() => {
                    this.Done();
                });
        }

        public SearchDealer(name: string) {

            var scope = this.$scope;

            var filter = new Queries.DealerQuery();
            filter.Name = name;
            filter.IsSimple = true; // Id/Name

            // Api call
            var promise = this.DealerService.Search(filter);

            // Parse data
            var result =
                promise
                    .then((response: Queries.PagedList<Models.Dealer>) => {
                        return response.Items;
                    })
                    .catch((error: Models.ErrorResult) => {
                        this.ToastService.Warning("There's an error fetching the data..");
                        return [];
                    });

            return result;
        }
    }
} 
