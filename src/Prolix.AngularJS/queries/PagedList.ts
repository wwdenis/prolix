// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Queries {
    'use strict';

    export interface PagedList<ModelType> extends PageRequest {
        Items: ModelType[];
        PageCount: number;
        RecordCount: number;
    }
}  
