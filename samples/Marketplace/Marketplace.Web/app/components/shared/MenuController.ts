// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../../_references.ts" />

module App.Controllers {
    'use strict';

    export class MenuController extends Controller {
        
        // $inject: Avoid injection errors caused by minification
        static $inject = ['$scope', '$location', '$window', 'IdentityService'];

        // Injected dependencies
        constructor(
            public $scope: Scopes.MenuScope,
            private $location: ng.ILocationService,
            private $window: ng.IWindowService,
            private IdentityService: Services.IdentityService
        ) {
            // Invoke parent constructor
            super($scope);

            // Watch for open/close state
            this.WatchMenu();
        }
        
        public Init() {
            this.Load();
        }

        public Load() {
            var menu = this.CreateMenu();

            var storage = this.$window.sessionStorage;
            var data = storage.getItem("MenuStatus")
            var state = JSON.parse(data);

            for (var key in state) {
                var menuItem = Utils.SearchArray(menu, i => i.Name == key);
                if (menuItem != null)
                    menuItem.IsOpened = state[key];
            }

            this.$scope.Items = menu;
            this.$scope.User = this.IdentityService.CurrentContext.Name;
        }
        
        private WatchMenu() {
            // Observa a colecao para manter o estado do menu
            this.$scope.$watch((i: Scopes.MenuScope) => i.Items, (newValues: any, oldValues: any, scope: Scopes.MenuScope) => {
                var state = {};
                var list = scope.Items;
                var storage = this.$window.sessionStorage;

                for (var i = 0; i < list.length; i++) {
                    var item = list[i];
                    state[item.Name] = item.IsOpened;
                }

                var stateJson = JSON.stringify(state);
                storage.setItem("MenuStatus", stateJson)
            }, true);
        }

        public CreateMenu(): Models.Menu[] {
            return [
                {
                    Name: "Configuration",
                    Caption: "Configuration",
                    Url: "",
                    IsOpened: false,
                    Icon: "fa fa-pencil-square-o",
                    Children: [
                        new Models.Menu("Categories", "Categories", "categories", "glyphicon glyphicon-wrench"),
                        new Models.Menu("Products", "Products", "products", "glyphicon glyphicon-heart"),
                    ]
                },
                {
                    Name: "Trading",
                    Caption: "Trading",
                    Url: "",
                    IsOpened: false,
                    Icon: "fa fa-money",
                    Children: [
                        new Models.Menu("Customers", "Customers", "customers", "glyphicon glyphicon-credit-card"),
                        new Models.Menu("Dealers", "Dealers", "dealers", "glyphicon glyphicon-home"),
                        new Models.Menu("Orders", "Orders", "orders", "glyphicon glyphicon-shopping-cart")
                    ]
                },
                {
                    Name: "Security",
                    Caption: "Security",
                    Url: "",
                    IsOpened: false,
                    Icon: "fa fa-unlock",
                    Children: [
                        new Models.Menu("User", "Users", "users", "glyphicon glyphicon-user"),
                        new Models.Menu("Role", "Roles", "roles", "glyphicon glyphicon-thumbs-up"),
                        new Models.Menu("Permission", "Permissions", "permissions", "glyphicon glyphicon-check"),
                        new Models.Menu("Log", "Audit Logs", "logs", "glyphicon glyphicon-indent-right")
                    ]
                }
            ];
        }
    }
}
