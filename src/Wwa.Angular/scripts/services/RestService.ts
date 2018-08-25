// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="Service.ts" />

module App.Services {
    "use strict";

    export abstract class RestService<ModelType extends Models.Model> extends Service {
        static $inject = ["$http", "$q", "Application"];

        public ApiController: string;

        constructor(
            protected $http: ng.IHttpService,
            protected $q: ng.IQService,
            protected Application: IApplication) {

            super($http, $q, Application);
        }

        public All(): ng.IPromise<ModelType[]> {

            var url = super.BuildUrl(this.ApiController);
            var promise = super.HttpGet<ModelType[]>(url);

            return promise;
        }

        public Get(id: number): ng.IPromise<ModelType> {
            var url = super.BuildUrl(this.ApiController, id);

            var promise = super.HttpGet<ModelType>(url);
            return promise;
        }
    }
}