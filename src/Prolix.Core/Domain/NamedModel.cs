// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

namespace Prolix.Core.Domain
{
    /// <summary>
    /// Named Model (Id, Name)
    /// </summary>
    public abstract class NamedModel : Model, INamed
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public NamedModel()
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="name">The model name</param>
        public NamedModel(string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Model name
        /// </summary>
        public virtual string Name { get; set; }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Generic text representation
        /// </summary>
        /// <returns>The model name.</returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
