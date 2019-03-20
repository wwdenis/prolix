// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Setup {
    "use strict";

    export class AuthorizationManager {

        static $inject = ["$rootScope", "$location", "$http", "$window", "IdentityService", "Application"];

        /**
         * Check the authentication status
         */
        public static Run($rootScope: Scopes.ViewScope, $location: ng.ILocationService, $http: ng.IHttpService, $window: ng.IWindowService, Application: IApplication, IdentityService: Services.IdentityService) {
            $rootScope.$on("$locationChangeStart", () => {
                AuthorizationManager.OnChangeLocation($rootScope, $location, Application, IdentityService);
            });
        }

        /**
         * Route validation handler
         */
        private static OnChangeLocation($rootScope: Scopes.ViewScope, $location: ng.ILocationService, Application: IApplication, IdentityService: Services.IdentityService) {
            var url = $location.path();
            var route = RouteManager.GetCurrent($location, Application.Routes);
            var isAuthenticated = IdentityService.IsAutheticated();

            if (!isAuthenticated) {
                if (route == null || !route.AllowAnonymous) {
                    $location.path(Application.LoginUrl);
                }
            }
        }
    }
}  
