// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Queries {
    'use strict';

    export class QueryRequest implements PageRequest, SortRequest {
        PageNumber: number;
        PageSize: number;
        SortField: string;
        SortDescending: boolean;
        IsSimple: boolean;

        constructor() {
            this.PageNumber = 1;
            this.PageSize = 10;
            
            this.SortField = "";
            this.SortDescending = false;
        }
    }
} 
