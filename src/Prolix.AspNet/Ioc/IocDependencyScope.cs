// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;

using Prolix.Ioc;

namespace Prolix.AspNet.Ioc
{
    public class IocDependencyScope : IDependencyScope
    {
        protected Resolver Resolver { get; }

        public IocDependencyScope(Resolver resolver)
        {
            Resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
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
