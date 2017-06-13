// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;

using Wwa.Core.Ioc;

namespace Wwa.Api.Ioc
{
    public class IocDependencyScope : IDependencyScope
    {
        protected IResolver Resolver { get; }

        public IocDependencyScope(IResolver resolver)
        {
            Resolver = resolver ?? throw new ArgumentNullException("resolver");
        }

        public object GetService(Type serviceType)
        {
            if (typeof(IHttpController).IsAssignableFrom(serviceType))
                return Resolver.Resolve(serviceType);

            if (!Resolver.IsRegistered(serviceType))
                return null;

            return Resolver.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Resolver.ResolveAll(serviceType);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Resolver.Dispose();
            }
        }
    }
}
