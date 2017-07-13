// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using AutoMapper;
using Newtonsoft.Json;

using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;

using Marketplace.Api.Core.Filters;
using Marketplace.Data;
using Marketplace.Domain.Security;
using Marketplace.Logic.Services.Configuration;

using Wwa.Api.Cors;
using Wwa.Api.Extensions;
using Wwa.Api.Filters;
using Wwa.Api.Formatters;
using Wwa.Api.Handlers;
using Wwa.Api.Ioc;
using Wwa.Api.Providers;
using Wwa.Core.Ioc;
using Wwa.Identity.AspNet;
using Wwa.Ioc.Autofac;

namespace Marketplace.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Dependency Resolver
            ConfigureDependencies(config);

            // Api Handlers/Services
            ConfigureHandlers(config);

            // Formatters
            ConfigureFormatters(config);

            // Web API routes
            ConfigureRoutes(config);

            // CORS
            ConfigureCors(config);

            // AutoMapper
            ConfigureMapper();
        }

        static void ConfigureDependencies(HttpConfiguration config)
        {
            // IoC container
            var manager = new AutofacDependencyManager();

            // Map app dependencies
            manager.MapAssembly<SecurityContext>();     // Domain
            manager.MapAssembly<DataContext>();         // Data
            manager.MapAssembly<CategoryService>();     // Logic
            manager.MapAssembly<PermissionAttribute>(); // Api

            // Map code dependencies
            manager.MapAssembly<GlobalAuthorizeAttribute>();// Filters
            manager.MapAssembly<IdentityManager>();         // Identity
            
            // Map all controllers
            manager.MapType<IHttpController>(Assembly.GetExecutingAssembly());

            // Setting the Resolver
            config.DependencyResolver = manager.GetHttpResolver();
        }
        
        static void ConfigureHandlers(HttpConfiguration config)
        {
            // Global Services/Handlers
            config.Services.Replace(typeof(IFilterProvider), new GlobalFilterProvider());
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            // Route interceptor
            config.MessageHandlers.Insert(0, new RouteHandler());
        }

        static void ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.MapHttpDefaultRoutes();
        }

		static void ConfigureFormatters(HttpConfiguration config)
		{
			config.Formatters.Add(new CsvMediaTypeFormatter());

			var jsonSettings = config?.Formatters?.JsonFormatter?.SerializerSettings;

			if (jsonSettings != null)
				jsonSettings.NullValueHandling = NullValueHandling.Ignore;
		}

        static void ConfigureCors(HttpConfiguration config)
        {
            config.EnableCors(new CorsPolicyProvider());
        }

        static void ConfigureMapper()
        {
            var assembly = typeof(WebApiConfig).Assembly;

            Mapper.Initialize(mapper => mapper.AddProfiles(assembly));
        }
    }
}
