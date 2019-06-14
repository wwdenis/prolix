// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Prolix.AspNet.Filters
{
    public class GlobalAuthorizeAttribute : AuthorizeAttribute, IDependencyFilter
    {
        public FilterScope Scope => FilterScope.Global;
    }
}
