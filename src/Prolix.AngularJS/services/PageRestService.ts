// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="RestService.ts" />

module App.Services {
    "use strict";

    export abstract class PageRestService<ModelType extends Models.Model, FilterType extends Queries.QueryRequest> extends RestService<ModelType> {
        static $inject = ["$http", "$q", "Application"];

        constructor(
            protected $http: ng.IHttpService,
            protected $q: ng.IQService,
            protected Application: IApplication) {
            super($http, $q, Application);
        }

        public Search(filter?: FilterType): ng.IPromise<Queries.PagedList<ModelType>> {

            var query = super.ToQueryString(filter);
            var url = super.BuildUrl(this.ApiController, query);

            var config = this.BuildConfig();
            var promise = super.HttpGet<Queries.PagedList<ModelType>>(url, config);

            return promise;
        }

        BuildConfig(): ng.IRequestShortcutConfig {
            var defaults = this.$http.defaults.transformResponse;
            var custom: ng.IHttpResponseTransformer = this.PagedListTransform;
            var transforms = Utils.ConcatArray<ng.IHttpResponseTransformer>(defaults, custom);

            var config: ng.IRequestShortcutConfig = {
                transformResponse: transforms
            };

            return config;
        }

        PagedListTransform<T>(data: any, headers: ng.IHttpHeadersGetter, status: number): any {

            var pageCountHeader = headers('X-Page-Count');
            var pageNumberHeader = headers('X-Page-Number');
            var pageSizeHeader = headers('X-Page-Size');
            var recordCountHeader = headers('X-Page-Records');

            if (!recordCountHeader)
                return null;

            var pageCount = parseInt(pageCountHeader);
            var pageNumber = parseInt(pageNumberHeader);
            var pageSize = parseInt(pageSizeHeader);
            var recordCount = parseInt(recordCountHeader);

            var page: Queries.PagedList<T> = {
                Items: <any>data,
                PageCount: pageCount,
                PageNumber: pageNumber,
                PageSize: pageSize,
                RecordCount: recordCount
            };

            return page;
        }
    }
}