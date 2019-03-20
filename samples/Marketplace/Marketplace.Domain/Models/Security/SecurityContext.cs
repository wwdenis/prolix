// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using Marketplace.Domain.Models.Security;
using Prolix.Core.Ioc;

namespace Marketplace.Domain.Security
{
    /// <summary>
    /// Current Session
    /// </summary>
    public sealed class SecurityContext : IInstance
    {
        /// <summary>
        /// Current User
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Current Feature
        /// </summary>
        public Feature Feature { get; set; }
    }
}
