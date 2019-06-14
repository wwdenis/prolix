// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

namespace Prolix.Domain
{
    /// <summary>
    /// Activable
    /// </summary>
    public interface IActivable
    {
        /// <summary>
        /// Active status
        /// </summary>
        bool Active { get; set; }
    }
}
