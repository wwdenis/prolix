// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="../_references.ts" />

module App.Models {
    'use strict';

    export class OrderItem extends Model {
        public Quantity: number;
        public Price: number;
        public Amount: number;

        public ProductId: number;
        public ProductName: string;
    }
} 
