// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Services {
    'use strict';

    export interface IService {
    }

    export class Service implements IService {

        protected Controller: string;

        constructor(
            protected $http: ng.IHttpService,
            protected $q: ng.IQService,
            protected Application: IApplication) {
        }

        protected BuildUrl(...urls: any[]) {
            var api = this.Application.ApiUrl;
            var url = Utils.UrlJoin(...urls);
            var result = Utils.UrlJoin(api, url);

            return result;
        }

        protected ToQueryString(data: any) {
            if (!data)
                return '';

            return Utils.ToQueryString(data);
        }

        protected Resolve<T>(httpPromise: ng.IHttpPromise<T>): ng.IPromise<T> {
            var deferred = this.$q.defer<T>();

            httpPromise
                .then(response => {
                    deferred.resolve(response.data || <T>response)
                })
                .catch(reason => {
                    deferred.reject(reason)
                });

            return deferred.promise;
        }

        protected HttpGet<T>(url: string, config?: ng.IRequestShortcutConfig): ng.IPromise<T> {
            var response = this.$http.get<T>(url, config);
            var promise = this.Resolve(response);
            return promise;
        }

        protected HttpPost<T>(url: string, data: any): ng.IPromise<T> {
            var response = this.$http.post<T>(url, data);
            var promise = this.Resolve(response);
            return promise;
        }

        protected HttpPut<T>(url: string, data: any): ng.IPromise<T> {
            var response = this.$http.put<T>(url, data);
            var promise = this.Resolve(response);
            return promise;
        }

        protected HttpDelete<T>(url: string): ng.IPromise<T> {
            var response = this.$http.delete<T>(url);
            var promise = this.Resolve(response);
            return promise;
        }
    }
}