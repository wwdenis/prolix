// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../_references.ts" />

module App.Queries {
    'use strict';

    export class CountryQuery extends QueryRequest {
        public Name: string;
        
        constructor() {
            super();

            this.SortField = "Name";
            this.PageNumber = 0;
            this.PageSize = 0;
        }
    }
}  
