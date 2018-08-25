// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../_References.ts" />

module App.Services {
    "use strict";

    export class CustomerService extends UpdateRestService<Models.Customer, Queries.CustomerQuery> {
        static $inject = ["$http", "$q", "Application"];

        constructor(public $http: ng.IHttpService, $q: ng.IQService, public Application: IApplication) {
            super($http, $q, Application);

            this.ApiController = "Customer";
        }
    }
}
