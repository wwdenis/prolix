// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Setup {
    "use strict";

    export class ControllerManager {

        static $inject = ["$controllerProvider", "Application"];

        static Configure($controllerProvider: ng.IControllerProvider, Application: IApplication) {
            var routes = Application.Routes;
            var menubar = Application.Menubar;
            var topbar = Application.Topbar;
            var names: boolean[] = [];

            var register = (controller: any) => {
                if (!controller)
                    return;

                var fn = <Function>controller;
                var name = Utils.GetFunctionName(fn);
                    
                // Cria a referencia para a controller se a mesma nao existir
                if (!names[name]) {
                    $controllerProvider.register(name, fn);
                    names[name] = true;
                }
            };

            if (!Utils.IsUndefined(menubar))
                register(menubar.Controller);

            if (!Utils.IsUndefined(topbar))
                register(topbar.Controller);

            angular.forEach(routes, (route, index) => {
                register(route.Controller);
            });
        }
    }
} 
