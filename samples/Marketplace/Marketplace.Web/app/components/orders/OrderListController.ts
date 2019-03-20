// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../../_references.ts" />

module App.Controllers {
    'use strict';

    export class OrderListController extends PagedController<Scopes.OrderListScope> {
        
        // $inject: Avoid injection errors caused by minification
        static $inject = ['$scope', '$location', '$timeout', 'DialogService', 'ToastService', 'CustomerService', 'DealerService', 'ProductService', 'StatusTypeService', 'OrderService'];

        // Injected dependencies
        constructor(
            public $scope: Scopes.OrderListScope,
            private $location: ng.ILocationService,
            private $timeout: ng.ITimeoutService,
            private DialogService: Services.DialogService,
            private ToastService: Services.ToastService,
            private CustomerService: Services.CustomerService,
            private DealerService: Services.DealerService,
            private ProductService: Services.ProductService,
            private StatusTypeService: Services.StatusTypeService,
            private OrderService: Services.OrderService
        ) {
            // Invoke parent constructor
            super($scope);
        }

        public Init() {
            // Api list criteria
            this.$scope.Customer = new Models.AutoCompleteConfig<Models.Customer>();
            this.$scope.Dealer = new Models.AutoCompleteConfig<Models.Dealer>();
            this.$scope.Product = new Models.AutoCompleteConfig<Models.Product>();
            this.$scope.Query = new Queries.OrderQuery();

            this.LoadStatusTypes();
        }

        public Add() {
            this.$location.url("/orders/add");
        }

        public Search() {
            var scope = this.$scope;
            var promise = this.OrderService.Search(scope.Query);

            this.Wait();

            promise
                .then((result: Queries.PagedList<Models.Order>) => {
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
            var promise = this.OrderService.Delete(id);

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

        private LoadStatusTypes() {

            var scope = this.$scope;

            var promise = this.StatusTypeService.All();

            this.Wait();

            promise
                .then((result: Models.StatusType[]) => {
                    scope.StatusTypes = result;
                })
                .catch((error: Models.ErrorResult) => {
                    scope.Rule = error.Rule;
                    this.ToastService.Warning(error.Text);
                })
                .finally(() => {
                    this.Done();
                });
        }

        public SearchCustomer(name: string) {

            var scope = this.$scope;

            var filter = new Queries.CustomerQuery();
            filter.Name = name;
            filter.IsSimple = true; // Id/Name

            // Api call
            var promise = this.CustomerService.Search(filter);

            // Parse data
            var result =
                promise
                    .then((response: Queries.PagedList<Models.Customer>) => {
                        return response.Items;
                    })
                    .catch((error: Models.ErrorResult) => {
                        this.ToastService.Warning("There's an error fetching the data..");
                        return [];
                    });

            return result;
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

        public SearchProduct(name: string) {
            
            var scope = this.$scope;

            var filter = new Queries.ProductQuery();
            filter.Name = name;
            filter.IsSimple = true; // Id/Name

            // Api call
            var promise = this.ProductService.Search(filter);

            // Parse data
            var result =
                promise
                    .then((response: Queries.PagedList<Models.Product>) => {
                        return response.Items;
                    })
                    .catch((error: Models.ErrorResult) => {
                        this.ToastService.Warning("There's an error fetching the data..");
                        return [];
                    });

            return result;
        }

        public StatusStyle(value: number) {
            // TODO: Parametrize
            return value > 1 ? 'fa fa-check' : 'fa fa-times';
        }
    }
} 
