// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

module App {
    "use strict";

    /**
    * Represents the application configuration.
    * It manages information about api comunication, routes, services, directives and filters
    */
    export class Application implements IApplication {

        constructor(apiUrl: string) {
            this.ApiUrl = apiUrl;
        }
        
        /**
         * Base Api for all http calls
         */
        public ApiUrl: string;
        
        /**
         * Angular app module name
         */
        public ModuleName: string = "App";
        
        /**
         * View root folder
         */
        public ViewRoot: string = "/app/components";

        /**
         * Controller alias used in all views
         */
        public ControllerAlias: string = "CT";

        /**
         * Login url used for redirection
         */
        public LoginUrl: string = "/login";

        /**
         * Diretive configuration
         */
        public Filters: Setup.FilterConfiguration[] = [
            { Name: "yesNo", Filter: Filters.YesNoFilter }
        ];

        /**
         * Filter configuration
         */
        public Directives: Setup.DirectiveConfiguration[] = [
            { Name: "waFocus",  Directive: Directives.FocusDirective },
            { Name: "waBlur",   Directive: Directives.BlurDirective },
            { Name: "waNumber", Directive: Directives.NumberDirective }
        ];

        /**
         * Service configuration
         */
        public Services: Services.IService[] = [
            Services.CategoryService,
            Services.CustomerService,
            Services.CountryService,
            Services.DealerService,
            Services.DialogService,
            Services.IdentityService,
            Services.ProductService,
            Services.ProvinceService,
            Services.RoleService,
            Services.OrderService,
            Services.StatusTypeService,
            Services.ToastService,
            Services.UserService
        ];

        /**
         * Route configuration
         */
        public Menubar: Setup.RouteConfiguration = {
            View: "/shared/menu.html",
            Controller: Controllers.MenuController
        };

        /**
         * Route configuration
         */
        public Topbar: Setup.RouteConfiguration = {
            View: "/shared/top.html",
            Controller: Controllers.TopController
        };

        /**
         * Route configuration
         */
        public Routes: Setup.RouteConfiguration[] = [
            {
                Url: "/",
                View: "/shared/main.html",
                Controller: Controllers.MainController
            },
            {
                Url: "/404",
                View: "/shared/404.html",
                IsNotFound: true,
                AllowAnonymous: true,
                MenuBar: null,
                TopBar: null
            },
            {
                Url: "/500",
                View: "/shared/500.html",
                IsInternalServerError: true,
                AllowAnonymous: true
            },
            {
                Url: "/login",
                View: "/identity/login.html",
                Controller: Controllers.LoginController,
                AllowAnonymous: true,
                MenuBar: null,
                TopBar: null
            },
            {
                Url: "/register",
                View: "/identity/register.html",
                Controller: Controllers.RegisterController,
                AllowAnonymous: true,
                MenuBar: null,
                TopBar: null
            },
            {
                Url: "/change-password",
                View: "/identity/changePassword.html",
                Controller: Controllers.ChangePasswordController
            },
            {
                Url: "/users",
                View: "/users/userList.html",
                Controller: Controllers.UserListController
            },
            {
                Url: "/users/add",
                View: "/users/userForm.html",
                Controller: Controllers.UserFormController
            },
            {
                Url: "/users/edit/:Id",
                View: "/users/userForm.html",
                Controller: Controllers.UserFormController
            },
            {
                Url: "/users/:Id",
                View: "/users/userDetail.html",
                Controller: Controllers.UserDetailController
            },
            {
                Url: "/categories",
                View: "/categories/categoryList.html",
                Controller: Controllers.CategoryListController
            },
            {

                Url: "/categories/add",
                View: "/categories/categoryForm.html",
                Controller: Controllers.CategoryFormController
            },
            {

                Url: "/categories/edit/:Id",
                View: "/categories/categoryForm.html",
                Controller: Controllers.CategoryFormController
            },
            {
                Url: "/categories/:Id",
                View: "/categories/categoryDetail.html",
                Controller: Controllers.CategoryDetailController
            },
            {
                Url: "/products",
                View: "/products/productList.html",
                Controller: Controllers.ProductListController
            },
            {
                Url: "/products/add",
                View: "/products/productForm.html",
                Controller: Controllers.ProductFormController
            },
            {
                Url: "/products/edit/:Id",
                View: "/products/productForm.html",
                Controller: Controllers.ProductFormController
            },
            {
                Url: "/products/:Id",
                View: "/products/productDetail.html",
                Controller: Controllers.ProductDetailController
            },
            {
                Url: "/customers",
                View: "/customers/customerList.html",
                Controller: Controllers.CustomerListController
            },
            {
                Url: "/customers/add",
                View: "/customers/customerForm.html",
                Controller: Controllers.CustomerFormController
            },
            {
                Url: "/customers/edit/:Id",
                View: "/customers/customerForm.html",
                Controller: Controllers.CustomerFormController
            },
            {
                Url: "/customers/:Id",
                View: "/customers/customerDetail.html",
                Controller: Controllers.CustomerDetailController
            },
            {
                Url: "/dealers",
                View: "/dealers/dealerList.html",
                Controller: Controllers.DealerListController
            },
            {
                Url: "/dealers/add",
                View: "/dealers/dealerForm.html",
                Controller: Controllers.DealerFormController
            },
            {
                Url: "/dealers/edit/:Id",
                View: "/dealers/dealerForm.html",
                Controller: Controllers.DealerFormController
            },
            {
                Url: "/dealers/:Id",
                View: "/dealers/dealerDetail.html",
                Controller: Controllers.DealerDetailController
            },
            {
                Url: "/orders",
                View: "/orders/orderList.html",
                Controller: Controllers.OrderListController
            },
            {
                Url: "/orders/add",
                View: "/orders/orderForm.html",
                Controller: Controllers.OrderFormController
            },
            {
                Url: "/orders/edit/:Id",
                View: "/orders/orderForm.html",
                Controller: Controllers.OrderFormController
            }
        ];
    }
} 
