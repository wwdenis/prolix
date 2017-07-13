// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Wwa.Core.Ioc;

namespace Wwa.Api.Ioc
{
    public static class ResolverExtensions
    {
        public static IDependencyResolver GetHttpResolver(this IResolverManager manager)
        {
            manager.Build();
            return new IocDependencyResolver(manager.Resolver);
        }
    }
}
