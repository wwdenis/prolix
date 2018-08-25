// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../_references.ts" />

module App.Directives {
    'use strict';

    export function BlurDirective(): ng.IDirective {
        return {
            link: ($scope: ng.IScope, element: ng.IAugmentedJQuery, attributes: ng.IAttributes) => {
                element.bind('blur', () => {
                    $scope.$apply(attributes['waBlur']);
                });

                $scope.$on('$destroy', () => {
                    element.unbind('blur');
                });
            }
        };
    }
}
