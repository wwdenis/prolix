// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Setup {
    "use strict";

    export class RouteManager {

        static $inject = ["$httpProvider", "$stateProvider", "$urlRouterProvider", "Application"];

        static Configure($httpProvider: ng.IHttpProvider, $stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider, Application: IApplication) {
            var routes = Application.Routes || [];
            var root = Application.ViewRoot || "/app/components";
            var alias = Application.ControllerAlias || "CT";
            
            var topbar = RouteManager.CreateMenu(Application.Topbar, Application.ViewRoot, Application.ControllerAlias);
            var menubar = RouteManager.CreateMenu(Application.Menubar, Application.ViewRoot, Application.ControllerAlias);

            var notFoundUrl = "/";

            // Views
            angular.forEach(routes, (route, index) => {
                var controller = Utils.GetFunctionName(route.Controller);

                var url = Utils.UrlJoin("/", route.Url, "/");
                var template = Utils.UrlJoin("/", root, route.View);

                var content: ng.ui.IState = {
                    templateUrl: template,
                    controller: controller,
                    controllerAs: alias
                }

                var menu: ng.ui.IState = {};
                var top: ng.ui.IState = {};
                
                if (Utils.IsUndefined(route.MenuBar))
                    menu = menubar;
                else
                    menu = RouteManager.CreateMenu(route.MenuBar, Application.ViewRoot, Application.ControllerAlias);

                if (Utils.IsUndefined(route.TopBar))
                    top = topbar;
                else
                    top = RouteManager.CreateMenu(route.TopBar, Application.ViewRoot, Application.ControllerAlias);

                if (Utils.EndsWith(url, "/") && url !== "/")
                    url = url.substring(0, url.length - 1);

                if (!Utils.IsEmpty(url)) {
                    var state: ng.ui.IState = {
                        url: url,
                        views: {
                            "content": content,
                            "menubar": menu,
                            "topbar": top
                        }
                    };

                    $stateProvider.state(url, state);
                }

                if (route.IsNotFound)
                    notFoundUrl = url;
            });

            // If the URL is not found, hows the configured one (otherwise method)
            $urlRouterProvider.otherwise(notFoundUrl);
        }

        private static NormalizeUrl($injector: ng.auto.IInjectorService, $location: ng.ILocationService) {
            var path = $location.path();
            var search = $location.search();
            var result = path;

            if (path[path.length - 1] !== "/") {
                result = path + "/";

                if (search !== {}) {
                    var params = [];

                    angular.forEach(search, (v, k) => {
                        params.push(k + "=" + v);
                    });

                    if (params.length > 0)
                        result = path + "?" + params.join("&");
                }
            }

            return result;
        }

        private static CreateMenu(config: Setup.RouteConfiguration, viewRoot: string, controllerAlias: string) : ng.ui.IState {
            if (!config)
                return {};

            var controller = Utils.GetFunctionName(config.Controller);
            var view = Utils.UrlJoin("/", viewRoot, config.View);
            var alias = config.ControllerAlias || controllerAlias || "";

            var menu = {
                templateUrl: view,
                controller: controller,
                controllerAs: alias
            };
            
            return menu;
        }

        public static GetCurrent($location: ng.ILocationService, routes: Setup.RouteConfiguration[]) {
            if (Utils.IsUndefined(routes))
                return null;

            var url = $location.path();

            var result = routes.filter((value, index, list) => {
                return value.Url == url;
            });

            if (result.length > 0)
                return result[0];

            return null;
        }

    }
} 
