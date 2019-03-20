// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Models {
    'use strict';

    export class DatePickerConfig {
        IsOpened: boolean;
        Options: ng.ui.bootstrap.IDatepickerConfig;

        constructor() {
            this.IsOpened = false;
            this.Options = {
                formatYear: 'yy',
                startingDay: 1
            };
        }

        public Open($event: ng.IAngularEvent) {
            $event.preventDefault();
            $event.stopPropagation();

            this.IsOpened = true;
        };
    }
} 
