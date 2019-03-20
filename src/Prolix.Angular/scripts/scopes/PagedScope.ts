// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Scopes {
    'use strict';

    export interface PagedScope extends ViewScope {
        Query: Queries.QueryRequest;

        IsFirst: boolean;
        IsLast: boolean;

        PageCount: number;
        RecordCount: number;
        PageHeader: number[];
        PageSizes: number[];
    }
} 
