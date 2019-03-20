// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Collections.Generic;
using Prolix.Core.Domain;

namespace Marketplace.Domain.Models.Security
{
    /// <summary>
    /// User Role
    /// </summary>
    public class Role : ActiveNamedModel
    {
        #region Properties

        /// <summary>
        /// Is Super User
        /// </summary>
        public bool IsAdmin { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// Users related to the role
        /// </summary>
        public virtual IList<User> Users { get; } = new List<User>();

        /// <summary>
        /// Features related to the role
        /// </summary>
        public virtual IList<Feature> Permissions { get; } = new List<Feature>();

        #endregion
    }
}
