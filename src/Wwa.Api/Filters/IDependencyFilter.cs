// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Web.Http.Filters;
using Wwa.Core.Ioc;

namespace Wwa.Api.Filters
{
    public interface IDependencyFilter : IFilter, ISharedService
    {
        FilterScope Scope { get; }
    }
}
