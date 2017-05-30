// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Models {
    'use strict';

    export class ActiveNamedModel extends NamedModel {
        Active: boolean;

        constructor() {
            super();
            this.Active = true;
        }
    }
} 
