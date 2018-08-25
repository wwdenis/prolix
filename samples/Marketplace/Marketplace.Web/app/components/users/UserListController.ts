// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../../_references.ts" />

module App.Controllers {
    'use strict';

    export class UserListController extends PagedController<Scopes.UserListScope> {
        
        // $inject: Avoid injection errors caused by minification
        static $inject = ['$scope', '$location', 'DialogService', 'ToastService', 'RoleService', 'ProvinceService', 'UserService'];

        // Injected dependencies
        constructor(
            public $scope: Scopes.UserListScope,
            private $location: ng.ILocationService,
            private DialogService: Services.DialogService,
            private ToastService: Services.ToastService,
            private RoleService: Services.RoleService,
            private ProvinceService: Services.ProvinceService,
            private UserService: Services.UserService
        ) {
            // Invoke parent constructor
            super($scope);
        }

        public Init() {
            // Api list criteria
            this.$scope.Query = new Queries.UserQuery();

            // Load lists
            this.LoadRoles();
            
            // List all
            this.Search();
        }

        public Add() {
            this.$location.url("/users/add");
        }

        public Search() {
            
            var scope = this.$scope;

            this.Wait();
            
            var promise = this.UserService.Search(scope.Query);

            promise
                .then((result: Queries.PagedList<Models.User>) => {
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
            
            var dialog = this.DialogService.Confirm("Deseja excluir este usuário?");

            dialog.then((result: App.Models.DialogResult) => {
                if (result == App.Models.DialogResult.Yes) {
                    this.Delete(id);
                }
            });
        }

        public Delete(id: any) {
            var scope = this.$scope;
            var promise = this.UserService.Delete(id);

            promise
                .then((result: any) => {
                    this.ToastService.Info("User excluído com sucesso!");
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

        private LoadRoles() {
            var scope = this.$scope;
            var promise = this.RoleService.All();
            
            this.Wait();

            promise
                .then((result: Models.Role[]) => {
                    scope.Roles = result;
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
