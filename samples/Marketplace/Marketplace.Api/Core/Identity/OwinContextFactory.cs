using Microsoft.Owin;
using System;
using System.Web;
using Wwa.Core.Ioc;

namespace Marketplace.Api.Core.Identity
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