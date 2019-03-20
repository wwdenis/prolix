// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Setup {
    'use strict';

    export interface RouteConfiguration {
        Url?: string;
        View?: string;
        Controller?: any;
        ControllerAlias?: string;
        TopBar?: RouteConfiguration;
        MenuBar?: RouteConfiguration;

        AllowAnonymous?: boolean;
        IsNotFound?: boolean;
        IsInternalServerError?: boolean;
    }
} 
