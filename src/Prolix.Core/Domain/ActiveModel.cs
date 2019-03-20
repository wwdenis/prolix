// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

namespace Prolix.Core.Domain
{
    /// <summary>
    /// Activable Model (Id, Active)
    /// </summary>
    public abstract class ActiveModel : Model, IActivable
    {
        #region Properties

        /// <summary>
        /// Active status
        /// </summary>
        public bool Active { get; set; }

        #endregion
    }
}
