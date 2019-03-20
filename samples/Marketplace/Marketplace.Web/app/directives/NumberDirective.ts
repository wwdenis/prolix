// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../_references.ts" />

module App.Directives {
    'use strict';

    export function NumberDirective(): ng.IDirective {
        return {
            require: "ngModel",
            restrict: "A",
            link: ($scope: ng.IScope, element: ng.IAugmentedJQuery, attributes: ng.IAttributes, controller: any /*ng.INgModelController*/) => {

                var inputValue = (value: string) => {
                    if (value) {
                        var digits = value.replace(/[^0-9,]/g, '');

                        if (digits !== value) {
                            controller.$setViewValue(digits);
                            controller.$render();
                        }
                        return parseFloat(digits);
                    }
                    return undefined;
                };

                controller.$parsers.push(inputValue);
            }
        };
    }

    NumberDirective.$inject = [ ];
}
