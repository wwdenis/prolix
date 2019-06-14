// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Prolix.AspNet.Filters;
using Prolix.Ioc;

namespace Prolix.AspNet.Providers
{
    public class GlobalFilterProvider : IFilterProvider
    {
        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var current = actionDescriptor.GetFilters();

            var filters = configuration?
                .DependencyResolver?
                .GetServices(typeof(IDependencyFilter))
                .Cast<IDependencyFilter>();

            if (filters != null)
            {
                var global = from i in filters
                             where i.Scope == FilterScope.Global
                             select i;

                foreach (var filter in global)
                {
                    current.Insert(0, filter);
                }
            }


            foreach (IDependencyFilter filter in current)
            {
                yield return new FilterInfo(filter, filter.Scope);
            }
        }
    }
}
