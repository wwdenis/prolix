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
    public class IocDependencyResolver : IocDependencyScope, IDependencyResolver
    {
        public IocDependencyResolver(IResolver resolver) : base(resolver)
        {
        }

        public IDependencyScope BeginScope()
        {
            var childContainer = Resolver.CreateChild();

            return new IocDependencyScope(childContainer);
        }
    }
}
