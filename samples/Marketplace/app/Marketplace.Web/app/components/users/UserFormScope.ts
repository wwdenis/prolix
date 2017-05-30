// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../../_references.ts" />

module App.Scopes {
    'use strict';

    export interface UserFormScope extends FormScope<Models.User> {
        Roles: Models.Role[];
    }
}  
