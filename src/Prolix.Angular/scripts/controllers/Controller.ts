// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Controllers {
    'use strict';

    export abstract class Controller {

        constructor(public $scope: Scopes.ViewScope) {
            $scope.IsBusy = false;
            $scope.Promises = new Array<boolean>();

            // Observa a colecao para determinar se a pagina ainda esta ocupada (IsBusy)
            $scope.$watchCollection((i: Scopes.ViewScope) => i.Promises, (newValues: any, oldValues: any, scope: Scopes.ViewScope) => {
                if (!scope.Promises)
                    scope.Promises = new Array<boolean>();

                scope.IsBusy = scope.Promises.length > 0;
            });
        }

        abstract Init(): void;

        public Remainder(maximum: number, text: string) {
            if (Utils.IsUndefined(text))
                return maximum;

            return maximum - text.length;
        }

        public Wait () {
            if (!this.$scope.Promises)
                this.$scope.Promises = new Array<boolean>();

            this.$scope.Promises.push(true);
        }

        public Done() {
            if (this.$scope.Promises)
                this.$scope.Promises.shift();
            else
                this.$scope.IsBusy = false;
        }

        public RuleMessage(field: string) {
            var rule = this.$scope.Rule;

            if (rule == null || rule.Errors == null || typeof rule === "undefined" || typeof rule.Errors === "undefined")
                return "";
            var message = "";

            angular.forEach(rule.Errors, (item, index) => {
                if (item.Name == field) {
                    message = item.Message;
                    return;
                }
            });

            return message;
        }

        public ClearRule(field: string) {
            var rule = this.$scope.Rule;

            if (rule == null || rule.Errors == null || typeof rule === "undefined" || typeof rule.Errors === "undefined")
                return;

            angular.forEach(rule.Errors, (item, index) => {
                if (item.Name == field) {
                    rule.Errors.splice(index, 1);
                    return;
                }
            });
        }

        public RuleStyle(field: string) {
            var rule = this.$scope.Rule;

            if (rule == null || rule.Errors == null || typeof rule === "undefined" || typeof rule.Errors === "undefined")
                return "";

            var style = "field-validation-valid";

            angular.forEach(rule.Errors, (item, index) => {
                if (item.Name == field) {
                    style = "field-validation-error msg-val";
                    return;
                }
            });

            return style;
        }
    }
} 
