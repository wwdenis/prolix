// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App {
    "use strict";

    export class Utils {
        public static IsEmpty(value: string): boolean {
            if (Utils.IsUndefined(value))
                return true;

            if (value == null || Utils.IsUndefined(value.trim()) || value.trim() == "")
                return true;

            return false;
        }

        public static IsUndefined(value: any): boolean {
            return typeof value === "undefined";
        }

        public static HasValue(value: any): boolean {
            return !Utils.IsUndefined(value) && value != null;
        }

        public static EndsWith(value: string, search: string): boolean {
            if (Utils.IsEmpty(value) || Utils.IsEmpty(search))
                return false;

            return value.slice(-search.length) == search;
        }

        public static StartsWith(value: string, search: string): boolean {
            if (Utils.IsEmpty(value) || Utils.IsEmpty(search))
                return false;

            return value.slice(0, search.length) == search;
        }

        public static GetFunctionName(fn: any): string {
            var f = typeof fn == "function";
            var s = f && ((fn.name && ["", fn.name]) || fn.toString().match(/function ([^\(]+)/));
            return (!f && "") || (s && s[1] || "");
        }

        public static ToQueryString(data: any) : string {
            if (typeof data === "undefined" && data == null)
                return "";
            
            var list = new Array<string>();
            
            for (var key in data) {
                if (data.hasOwnProperty(key)) {
                    var value = data[key];

                    if (value instanceof Date)
                        value = value.toISOString();

                    if (typeof value !== "undefined")
                        list.push(key + "=" + value);
                }
            }

            if (list.length == 0)
                return "";

            var url = "?" + list.join("&");
            return url;
        }

        public static Copy<T extends Object>(source: T, destination: T) {

            if (typeof destination === "undefined")
                destination = <any>{};

            for (var attr in source) {
                if (source.hasOwnProperty(attr))
                    destination[attr] = source[attr];
            }
        }

        public static Clone(obj: any) {
            var copy;

            // Handle the 3 simple types, and null or undefined
            if (null == obj || "object" != typeof obj) return obj;

            // Handle Date
            if (obj instanceof Date) {
                copy = new Date();
                copy.setTime(obj.getTime());
                return copy;
            }

            // Handle Array
            if (obj instanceof Array) {
                copy = [];
                for (var i = 0, len = obj.length; i < len; i++) {
                    copy[i] = Utils.Clone(obj[i]);
                }
                return copy;
            }

            // Handle Object
            if (obj instanceof Object) {
                copy = {};
                for (var attr in obj) {
                    if (obj.hasOwnProperty(attr))
                        copy[attr] = Utils.Clone(obj[attr]);
                }
                return copy;
            }

            throw new Error("Unable to copy obj! Its type isn't supported.");
        }

        public static ReplaceAll(text: string, search: string, replace: string) : string {
            if (text.indexOf(search, 0) < 0)
                return text;

            var replaced = text.replace(search, replace);
            var result = Utils.ReplaceAll(replaced, search, replace);

            return result;
        }

        public static UrlJoin(...urls: any[]) : string {
            if (Utils.IsUndefined(urls) || urls.length == 0)
                return "";
            
            var result = urls.join("/");

            result = Utils.ReplaceAll(result, "//", "/");
            result = result.replace(":/", "://");
            
            return result;
        }

        public static ConcatArray<ObjectType>(...source: any[]): ObjectType[] {

            var destination = new Array();

            if (Utils.IsUndefined(source))
                return destination;


            for (var i = 0; i < source.length; i++) {
                var item = source[i];
                // We can't guarantee that the object is an array
                if (angular.isArray(item))
                    destination = destination.concat(item);
                else
                    // Append the new item to the array
                    destination.push(item);
            }
            
            return destination;
        }
        public static SearchArray<ObjectType>(list: ObjectType[], expression: (item: ObjectType) => boolean): ObjectType {
            if (Utils.IsUndefined(list) || Utils.IsUndefined(expression))
                return null;

            for (var i = 0; i < list.length; i++) {
                var item = list[i];
                if (expression(item))
                    return item;
            }

            return null;
        }

        public static Equals(first: any, second: any) {
            return !Utils.IsUndefined(first) && !Utils.IsUndefined(second) && first == second;
        }

        public static GetJson<JsonType>(fileUrl: string, validate: boolean): JsonType {
            var success = false;
            var result: JsonType;

            if (!Utils.IsEmpty(fileUrl)) {
                var xhr = new XMLHttpRequest();
                xhr.open("GET", fileUrl, false);
                xhr.send();
                success = (xhr.status == 200);
            }

            if (!success)
                throw new Error("File not found!");

            try {
                result = JSON.parse(xhr.responseText);
            }
            catch (e) {
                if (validate)
                    throw e;
                else
                    console.log(e.message);
            }

            return result;
        }

        static GetSettings(file: string) {
            var settings = Utils.GetJson<Models.AppSettings>(file, true);

            if (Utils.IsUndefined(settings) || Utils.IsEmpty(settings.ApiUrl)) {
                throw new Error("Invalid configuration file");
            }

            return settings;
        }
    }
}  
