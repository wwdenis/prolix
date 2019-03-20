// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Scopes {
    'use strict';

    export interface ViewScope extends ng.IScope {
        Rule: Models.RuleValidation;
        IsBusy: boolean;
        Promises: boolean[];
    }
} 
