// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Setup {
    "use strict";

    /**
     * Application Bootstraper
     */
    export class AppStart {

        static Run(config: IApplication) {
            
            var dependencies = [
                "ngRoute",
                "ngAnimate",
                "ngCookies",
                "ngTouch",
                "ngSanitize",
                "ngResource",
                "ui.router",
                "ui.bootstrap",
                "ui.mask"
            ];

            // Start the app module
            var app = angular.module(config.ModuleName, dependencies);

            // Register the Application config constant
            app.constant("Application", config);

            // Configure api interceptor
            app.config(ApiHttpInterceptor.configure);

            // Configure controllers
            app.config(ControllerManager.Configure);

            // Configure routes
            app.config(RouteManager.Configure);

            // Configure authentication
            app.run(AuthorizationManager.Run);

            // Configure services
            AppStart.ConfigureServices(app, config.Services);

            // Configure directives
            AppStart.ConfigureDirectives(app, config.Directives);

            // Configure filters
            AppStart.ConfigureFilters(app, config.Filters);
        }

        static ConfigureServices(app: ng.IModule, services: Services.IService[]) {
            var names: boolean[] = [];

            angular.forEach(services, (service, index) => {
                var svc: any = service;
                var fn = <Function>svc;
                var name = Utils.GetFunctionName(fn);

                // Create a service reference
                if (!Utils.IsUndefined(fn) && !Utils.IsEmpty(name) && !names[name]) {
                    app.service(name, fn);
                    names[name] = true;
                }
            });
        }

        static ConfigureDirectives(app: ng.IModule, directives: DirectiveConfiguration[]) {
            var names: boolean[] = [];

            angular.forEach(directives, (directive, index) => {
                var fn: any = directive.Directive;
                var name = directive.Name;

                // Create a directive reference
                if (!names[name] && !Utils.IsUndefined(fn)) {
                    app.directive(name, fn);
                    names[name] = true;
                }
            });
        }

        static ConfigureFilters(app: ng.IModule, filters: FilterConfiguration[]) {
            var names: boolean[] = [];

            angular.forEach(filters, (filter, index) => {

                var fn: any = filter.Filter;
                var name = filter.Name;

                // Create a filter reference
                if (!names[name] && !App.Utils.IsUndefined(fn)) {
                    var factory = () => fn;

                    app.filter(name, factory);
                    names[name] = true;
                }
            });
        }
    }
}
