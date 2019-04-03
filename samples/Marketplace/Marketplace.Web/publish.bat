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
COPY ..\node_modules\toastr\toastr.js
COPY ..\node_modules\awesome-bootstrap-checkbox\awesome-bootstrap-checkbox.css
COPY ..\node_modules\angular-ui-utils\modules\mask\mask.js

COPY ..\node_modules\dcjqaccordion\js\jquery.dcjqaccordion.2.7.js
COPY ..\node_modules\jquery.nicescroll\jquery.nicescroll.js
COPY ..\node_modules\jquery.scrollto\jquery.scrollto.js

MD bootstrap
XCOPY ..\node_modules\bootstrap\dist bootstrap\ /S

MD font-awesome
XCOPY ..\node_modules\font-awesome font-awesome\ /S


:: Prolix Core TS files
XCOPY ..\..\..\..\src\Prolix.AngularJS prolix-angularjs\ /S