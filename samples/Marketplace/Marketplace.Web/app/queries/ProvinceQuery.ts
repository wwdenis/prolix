// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../_references.ts" />

module App.Queries {
    'use strict';

    export class ProvinceQuery extends QueryRequest {
        Name: string;
        CountryId: number;

        constructor() {
            super();

            this.SortField = "Name";
            this.PageNumber = 0;
            this.PageSize = 0;
        }
    }
}  
