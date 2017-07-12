using System;
using System.Collections.Generic;
using System.Text;
using Wwa.Core.Ioc;

namespace Wwa.Ioc.Unity
{
    public class UnityResolverManager : ResolverManager
    {
        public UnityResolverManager() : base(new UnityResolver())
        {
        }
    }
}
