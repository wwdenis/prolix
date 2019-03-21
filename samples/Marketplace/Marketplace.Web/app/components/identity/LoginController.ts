// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Controllers {
    'use strict';

    export class LoginController extends Controller {

        // $inject: Avoid injection errors caused by minification
        static $inject = ['$scope', '$location', 'ToastService', 'IdentityService'];

        // Injected dependencies
        constructor(
            public $scope: Scopes.LoginScope,
            private $location: ng.ILocationService,
            private ToastService: Services.ToastService,
            private IdentityService: Services.IdentityService) {
            super($scope);
        }

        public Init() {
            this.$scope.Input = new Models.Login();

            if (this.IdentityService.IsAutheticated()) {
                this.$location.path("/");
            } 
            else if (!Utils.IsUndefined(this.IdentityService.CurrentContext)) {
                this.$scope.Input.UserName = this.IdentityService.CurrentContext.UserName;
            }

            //this.IdentityService.Logout();
        }

        public Login() {
            // Setando a propriedade "IsBusy" como TRUE
            this.Wait();

            // Montando o JSON
            var input = this.$scope.Input;

            // Invocando a API
            var promise = this.IdentityService.Login(input);

            // Tratando o resultado
            promise
                .then((data: Models.Access) => {
                    this.IdentityService.CreateContext(data, this.$scope.Remember);
                    this.$location.path("/");
                })
                .catch((error: Models.ErrorResult) => {
                    this.IdentityService.ClearContext();
                    this.$scope.Rule = error.Rule;
                    this.ToastService.Warning(error.Text);
                })
                .finally(() => {
                    this.Done();
                });
        }
    }
}
