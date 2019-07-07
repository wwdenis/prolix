// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Microsoft.Owin;
using System;
using System.Web;
using Prolix.Ioc;

namespace Marketplace.Api.Infrastructure
{
    public class OwinContextFactory : IFactory
    {
        public Type Type => typeof(IOwinContext);

        public object CreateInstance()
        {
            return HttpContext.Current.GetOwinContext();
        }
    }
}