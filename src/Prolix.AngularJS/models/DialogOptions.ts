// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Models {
    "use strict";

    export interface DialogOptions {
        HeaderText?: string;
        BodyText?: string;

        OkText?: string;
        CloseText?: string;

        OkEnabled?: boolean;
        CloseEnabled?: boolean;
        
        OkResult?: DialogResult;
        CloseResult?: DialogResult;
    }
}
