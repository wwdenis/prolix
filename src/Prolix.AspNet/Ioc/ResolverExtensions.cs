// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Web.Http.Dependencies;
using Prolix.Core.Ioc;

namespace Prolix.AspNet.Ioc
{
    public static class ResolverExtensions
    {
        public static IDependencyResolver GetHttpResolver(this Resolver resolver)
        {
            resolver.Build();
            return new IocDependencyResolver(resolver);
        }
    }
}
