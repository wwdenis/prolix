// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../_references.ts" />

module App.Directives {
    'use strict';

    export function FocusDirective($timeout: ng.ITimeoutService): ng.IDirective {
        return {
            link: ($scope: ng.IScope, element: ng.IAugmentedJQuery, attributes: ng.IAttributes) => {
                $scope.$watch("waFocus", function (newValue: string, oldValue: string, scope: ng.IScope) {
                    $timeout(() => element[0].focus(), 0, false);
                });
            }
        };
    }

    FocusDirective.$inject = ['$timeout'];
}
