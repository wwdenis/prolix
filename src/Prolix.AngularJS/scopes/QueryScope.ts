// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Scopes {
    'use strict';

    export interface QueryScope<ModelType extends Models.Model, QueryType extends Queries.QueryRequest> extends PagedScope {
        Query: QueryType;
        Items: ModelType[];
    }
} 
