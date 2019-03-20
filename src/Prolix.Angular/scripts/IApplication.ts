// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App {
    "use strict";

    /**
    * Represents the application configuration.
    * It manages information about api comunication, routes, services, directives and filters
    */
    export interface IApplication {

        /**
         * Base Api for all http calls
         */
        ApiUrl: string;

        /**
         * Angular app module name
         */
        ModuleName: string;

        /**
         * View root folder
         */
        ViewRoot: string;

        /**
         * Controller alias used in all views
         */
        ControllerAlias: string;

        /**
         * Login url used for redirection
         */
        LoginUrl: string;

        /**
         * Diretive configuration
         */
        Directives: Setup.DirectiveConfiguration[];

        /**
         * Filter configuration
         */
        Filters: Setup.FilterConfiguration[];

        /**
         * Service configuration
         */
        Services: Services.IService[];

        /**
         * Route configuration
         */
        Menubar: Setup.RouteConfiguration;

        /**
         * Route configuration
         */
        Topbar: Setup.RouteConfiguration;

        /**
         * Route configuration
         */
        Routes: Setup.RouteConfiguration[];
    }
}
