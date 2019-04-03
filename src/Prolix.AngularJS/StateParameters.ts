// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App {
    'use strict';

    export class StateParameters implements angular.ui.IStateParamsService {

        public Get<T>(parameter: string): T {
            var value = <T>this[parameter];

            if (typeof value === "undefined")
                return null;

            return value;
        }

        public get Id(): number {
            var value = this.Get<number>("Id");

            if (value == null)
                return 0;

            return value;
        }
    }
} 
