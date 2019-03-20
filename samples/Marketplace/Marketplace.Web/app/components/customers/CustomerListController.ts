// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../../_references.ts" />

module App.Controllers {
    'use strict';

    export class CustomerListController extends PagedController<Scopes.CustomerListScope> {
        
        // $inject: Avoid injection errors caused by minification
        static $inject = ['$scope', '$location', 'DialogService', 'ToastService', 'ProvinceService', 'CountryService', 'CustomerService'];

        // Injected dependencies
        constructor(
            public $scope: Scopes.CustomerListScope,
            private $location: ng.ILocationService,
            private DialogService: Services.DialogService,
            private ToastService: Services.ToastService,
            private ProvinceService: Services.ProvinceService,
            private CountryService: Services.CountryService,
            private CustomerService: Services.CustomerService
        ) {
            // Invoke parent constructor
            super($scope);
            
            // Reload child list
            $scope.$watch((i: Scopes.CustomerListScope) => i.Query.CountryId, (newValue: number, oldValue: number, scope: Scopes.CustomerListScope) => {
                if (!Utils.IsUndefined(newValue) && newValue != oldValue)
                    this.LoadProvinces(newValue);
            });
        }

        public Add() {
            this.$location.url("/customers/add");
        }

        public Init() {
            // Api list criteria
            this.$scope.Query = new Queries.CustomerQuery();

            // Load lists
            this.LoadCountries();
        
            // List all
            this.Search();
        }

        public Search() {
            var scope = this.$scope;
            var promise = this.CustomerService.Search(scope.Query);

            this.Wait();

            promise
                .then((result: Queries.PagedList<Models.Customer>) => {
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
            
            var dialog = this.DialogService.Confirm("Do you want to delete this customer?");

            dialog.then((result: App.Models.DialogResult) => {
                if (result == App.Models.DialogResult.Yes) {
                    this.Delete(id);
                }
            });
        }

        public Delete(id: any) {
            var scope = this.$scope;
            var promise = this.CustomerService.Delete(id);

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

        private LoadProvinces(countryId: number) {
            var scope = this.$scope;

            var filter = new Queries.ProvinceQuery();
            filter.CountryId = countryId;

            var promise = this.ProvinceService.Search(filter);

            this.Wait();

            promise
                .then((result: Queries.PagedList<Models.Province>) => {
                    scope.Provinces = result.Items;
                })
                .catch((error: Models.ErrorResult) => {
                    scope.Rule = error.Rule;
                    this.ToastService.Warning(error.Text);
                })
                .finally(() => {
                    this.Done();
                });
        }

        private LoadCountries() {
            
            var scope = this.$scope;

            var promise = this.CountryService.All();

            this.Wait();

            promise
                .then((result: Models.Country[]) => {
                    scope.Countries = result;
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
