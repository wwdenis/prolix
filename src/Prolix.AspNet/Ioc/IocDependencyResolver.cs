// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Web.Http.Dependencies;

using Prolix.Core.Ioc;

namespace Prolix.AspNet.Ioc
{
    public class IocDependencyResolver : IocDependencyScope, IDependencyResolver
    {
        public IocDependencyResolver(Resolver resolver) : base(resolver)
        {
        }

        public IDependencyScope BeginScope()
        {
            var childContainer = Resolver.CreateChild();

            return new IocDependencyScope(childContainer);
        }
    }
}
