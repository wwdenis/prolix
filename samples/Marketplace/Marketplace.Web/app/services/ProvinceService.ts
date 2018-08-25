// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../_references.ts" />

module App.Services {
    'use strict';

    export class ProvinceService extends PageRestService<Models.Province, Queries.ProvinceQuery> {
        static $inject = ["$http", "$q", "Application"];

        constructor(public $http: ng.IHttpService, $q: ng.IQService, public Application: IApplication) {
            super($http, $q, Application);
            this.ApiController = "Province";
        }
    }
} 
