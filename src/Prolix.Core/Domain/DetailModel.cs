// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

namespace Prolix.Core.Domain
{
    /// <summary>
    /// Detailed model (Id, Name, Detail)
    /// </summary>
    public abstract class DetailModel : NamedModel, IDetailed
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public DetailModel()
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="name">Model name</param>
        public DetailModel(string name) : base(name)
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="name">Model name</param>
        /// <param name="detail">Model detail</param>
        public DetailModel(string name, string detail) : this(name)
        {
            Detail = detail;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Detail
        /// </summary>
        public string Detail { get; set; }

        #endregion
    }
}
