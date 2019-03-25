ECHO COPYING SCRIPTS...
IF EXIST lib DEL /F /Q /S lib
MD lib
CD lib
COPY ..\node_modules\jquery\dist\jquery.min.js
COPY ..\node_modules\angular\angular.min.js
COPY ..\node_modules\angular-route\angular-route.min.js
COPY ..\node_modules\angular-ui-router\release\angular-ui-router.min.js
COPY ..\node_modules\angular-cookies\angular-cookies.min.js
COPY ..\node_modules\angular-animate\angular-animate.min.js
COPY ..\node_modules\angular-ui-bootstrap\dist\ui-bootstrap-tpls.js
COPY ..\node_modules\angular-ui-utils\modules\utils.js
COPY ..\node_modules\angular-resource\angular-resource.min.js
COPY ..\node_modules\angular-sanitize\angular-sanitize.min.js
COPY ..\node_modules\angular-touch\angular-touch.min.js
COPY ..\node_modules\bootstrap\dist\js\bootstrap.min.js
COPY ..\node_modules\toastr\toastr.js
COPY ..\node_modules\awesome-bootstrap-checkbox\awesome-bootstrap-checkbox.css
COPY ..\node_modules\angular-ui-utils\modules\mask\mask.js

:: Prolix Core TS files
XCOPY ..\..\..\..\src\Prolix.Angular\scripts prolix-angular\ /S