// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App.Services {
    "use strict";

    export class ToastService extends Service {
        static $inject = ["$http", "$q", "Application"];

        constructor(public $http: ng.IHttpService, $q: ng.IQService, public Application: IApplication) {
            super($http, $q, Application);

            var options = {
                timeOut: 3000,
                positionClass: "toast-bottom-right",
                preventDuplicates: true,
                progressBar: true
            };

            var w: any = window;

            // Set options third-party lib
            this.toast = w.toastr;
            this.toast.options = options;
        }

        private toast: any;

        public Warning(message) {
            return this.toast.warning(message);
        }

        public Success(message: string) {
            return this.toast.success(message);
        }

        public Info(message: string) {
            return this.toast.info(message);
        }

        public Clear() {
            return this.toast.clear();
        }

        public NoRecords() {
            var message = "There are no records for given criteria.";
            return this.toast.info(message);
        }

        public Deleted() {
            var message = "Entry successfully deleted.";
            return this.toast.info(message);
        }
    }
}
