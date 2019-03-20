// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Web.Http.Dependencies;
using Prolix.Core.Ioc;

namespace Prolix.Api.Ioc
{
    public static class ResolverExtensions
    {
        public static IDependencyResolver GetHttpResolver(this IDependencyManager manager)
        {
            manager.Build();
            return new IocDependencyResolver(manager.Resolver);
        }
    }
}
