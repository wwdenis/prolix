using Microsoft.Owin;
using System;
using System.Web;
using Prolix.Ioc;

namespace Marketplace.Api.Infrastructure.Identity
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