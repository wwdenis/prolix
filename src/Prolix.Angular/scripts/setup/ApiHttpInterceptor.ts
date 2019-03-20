// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Setup {
    "use strict";

    export class ApiHttpInterceptor implements ng.IHttpInterceptor {
        // @ngInject
        static factory($q: ng.IQService, $log: ng.ILogService, $location: ng.ILocationService, $injector: ng.auto.IInjectorService): ApiHttpInterceptor {
            return new ApiHttpInterceptor($q, $log, $location, $injector);
        }

        static configure($httpProvider: ng.IHttpProvider) {
            /* push the factory function to the array of $httpProvider
             * interceptors (implements IHttpInterceptorFactory) */
            $httpProvider.interceptors.push(ApiHttpInterceptor.factory);
        }

        private static log($log: ng.ILogService, rejection: ng.IHttpPromiseCallbackArg<any>) {
            var details = new Array();
            details.push("API Error:");
            details.push("Url: " + rejection.config.url);
            details.push("Detail: " + rejection.statusText);
            var message = details.join("\n");
            $log.error(message);
        }

        constructor(private $q: ng.IQService, private $log: ng.ILogService, private $location: ng.ILocationService, private $injector: ng.auto.IInjectorService) {
        }

        // created as instance method using arrow function
        request = (config: ng.IRequestConfig): ng.IRequestConfig => {
            // modify config
            return config;
        };

        // created as instance method using arrow function
        response = <T>(response: ng.IHttpPromiseCallbackArg<T>): ng.IPromise<T> => {
            var pos = response.config.url.indexOf('http');

            if (pos == 0) {
                return this.$q.when(response.data);
            }
            
            return this.$q.when(response);
        };

        requestError = (rejection: any) : any => {
            // do something on error
            //if (canRecover(rejection)) {
            //    return responseOrNewPromise
            //}
            return this.$q.reject(rejection);
        };

        responseError = (rejection: ng.IHttpPromiseCallbackArg<any>) : any => {
            var result: Models.ErrorResult = {
                IsBad: false,
                IsDenied: false,
                IsForbidden: false,
                IsServer: false,
                Text: "The server was unable to process the request.",
                Rule: null
            };

            var isHandled = false;

            switch (rejection.status) {
                case 304: // NotModified
                    result.Text = "There was no changes.";
                    break;
                case 400: // NotFound
                    isHandled = true;
                    result.IsBad = true;

                    if (Utils.HasValue(rejection.data)) {
                        result.Rule = rejection.data;
                        result.Text = result.Rule.Message;
                    }
                    break;
                case 401: // Denied
                    isHandled = true;
                    result.IsDenied = true;

                    // Calling injector do avoid circular references
                    var IdentityService = <Services.IdentityService>this.$injector.get('IdentityService');

                    // Go to login page
                    IdentityService.ClearContext();
                    this.$location.path("/");

                    break;
                case 403: // Forbidden
                    isHandled = true;
                    result.IsForbidden = true;
                    break;
                case 500: // InternalServerError
                    result.IsServer = true;
                    ApiHttpInterceptor.log(this.$log, rejection);
                    break;
                default:
                    ApiHttpInterceptor.log(this.$log, rejection);
                    break;
            }

            //if (isHandled)
            //    console.clear();

            return this.$q.reject(result);
        }
    }
}   
