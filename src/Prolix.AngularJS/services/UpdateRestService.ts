// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="PageRestService.ts" />

module App.Services {
    "use strict";

    export abstract class UpdateRestService<ModelType extends Models.Model, FilterType extends Queries.QueryRequest> extends PageRestService<ModelType, FilterType> {
        static $inject = ["$http", "$q", "Application"];

        constructor(
            protected $http: ng.IHttpService,
            protected $q: ng.IQService,
            protected Application: IApplication) {
            super($http, $q, Application);
        }

        public Add(item: ModelType): ng.IPromise<any> {
            var url = super.BuildUrl(this.ApiController);

            var promise = super.HttpPost(url, item);
            return promise;
        }

        public Edit(item: ModelType): ng.IPromise<any> {
            var url = super.BuildUrl(this.ApiController, item.Id);

            var promise = super.HttpPut(url, item);
            return promise;
        }

        public Save(item: ModelType): ng.IPromise<any> {
            if (item.Id > 0)
                return this.Edit(item);
            else
                return this.Add(item);
        }

        public Delete(id: number): ng.IPromise<any> {
            var url = super.BuildUrl(this.ApiController, id);

            var promise = super.HttpDelete(url);
            return promise;
        }
    }
}