// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

namespace Prolix.Core.Domain
{
    /// <summary>
    /// Named Activable Model (Id, Active, Name)
    /// </summary>
    public abstract class ActiveNamedModel : NamedModel, IActivable
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ActiveNamedModel()
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="name">Model name</param>
        public ActiveNamedModel(string name) : base(name)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Active status
        /// </summary>
        public bool Active { get; set; }

        #endregion
    }
}
