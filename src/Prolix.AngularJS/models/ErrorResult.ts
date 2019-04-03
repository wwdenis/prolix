// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Models {
    "use strict";

    export interface ErrorResult {
        IsBad: boolean;
        IsDenied: boolean;
        IsForbidden: boolean;
        IsServer: boolean;
        Text: string;
        Rule: RuleValidation;
    }
}   
