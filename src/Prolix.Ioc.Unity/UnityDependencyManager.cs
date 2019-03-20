using System;
using System.Collections.Generic;
using System.Text;
using Prolix.Core.Ioc;

namespace Prolix.Ioc.Unity
{
    public class UnityDependencyManager : DependencyManager
    {
        public UnityDependencyManager() : base(new UnityResolver())
        {
        }
    }
}
