// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Services {
    'use strict';

    export interface IService {
    }

    export class Service implements IService {

        public Controller: string;

        constructor(
            protected $http: ng.IHttpService,
            protected Application: IApplication) {
        }

        public BuildUrl(...urls: any[]) {
            var api = this.Application.ApiUrl;
            var url = Utils.UrlJoin(...urls);
            var result = Utils.UrlJoin(api, url);

            return result;
        }

        public ToQueryString(data: any) {
            if (!data)
                return '';

            return Utils.ToQueryString(data);
        }
    }
} 
