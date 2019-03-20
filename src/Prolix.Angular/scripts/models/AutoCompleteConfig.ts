// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Models {
    'use strict';

    export class AutoCompleteConfig<ModelType extends NamedModel> {
        Model: ModelType;
        IsLoading: boolean;
        NoResults: boolean;

        constructor() {
            var empty = new NamedModel();

            this.Model = <ModelType>empty;
            this.IsLoading = false;
            this.NoResults = false;
        }
    }
} 
