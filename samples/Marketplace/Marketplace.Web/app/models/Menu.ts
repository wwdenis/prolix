// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Models {
    'use strict';

    export class Menu {
        Name: string;
        Caption: string;
        Url: string;
        Icon: string;
        IsOpened: boolean;
        Children: Menu[];

        constructor(name?: string, caption?: string, url?: string, icon?: string, children?: Menu[]) {
            this.Name = name;
            this.Caption = caption;
            this.Url = url;
            this.Children = children;
            this.Icon = icon;
        }
    }
} 
