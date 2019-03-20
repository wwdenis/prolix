// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Core.Ioc
{
    /// <summary>
    /// Defines the dependency resolve lifetime
    /// </summary>
    public enum DepedencyLifetime
    {
        /// <summary>
        /// A new instance will be returned from each resolve request
        /// </summary>
        PerDependency = 1,

        /// <summary>
        /// A unique instance will be returned from all resolve requests
        /// </summary>
        PerLifetime = 2
    }
}
