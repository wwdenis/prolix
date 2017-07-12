using System;
using System.Collections.Generic;
using System.Text;
using Wwa.Core.Ioc;

namespace Wwa.Ioc.Autofac
{
    public class AutofacResolverManager : ResolverManager
    {
        public AutofacResolverManager() : base(new AutofacResolver())
        {
        }
    }
}
