// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Scopes {
    'use strict';

    export interface FormScope<ModelType extends Models.Model> extends ViewScope {
        Id: number;
        Form: ModelType;
    }
} 
