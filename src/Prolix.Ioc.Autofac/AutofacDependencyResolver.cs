using System;
using System.Collections.Generic;
using System.Text;
using Prolix.Core.Ioc;

namespace Prolix.Ioc.Autofac
{
    public class AutofacDependencyManager : DependencyManager
    {
        public AutofacDependencyManager() : base(new AutofacResolver())
        {
        }
    }
}
