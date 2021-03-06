// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

/// <reference path="_References.ts" />

module App.Setup {
    "use strict";

    /*
     * Application bootstrap
     */
    (() => {
        var settings = Utils.GetSettings("/config.json");
        var app = new Application(settings.ApiUrl);

        AppStart.Run(app);
    })();
}
