// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

namespace Prolix.Core.Domain
{
    /// <summary>
    /// Detailed Activable Model (Id, Active, Name, Detail)
    /// </summary>
    public abstract class ActiveDetailModel : DetailModel, IActivable
    {
        #region Constructors
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ActiveDetailModel()
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="name">Model name</param>
        public ActiveDetailModel(string name) : base(name)
        {
        }

        /// <summary>
        /// Instancia uma nova entidade
        /// </summary>
        /// <param name="name">Model name</param>
        /// <param name="detail">Model detail</param>
        public ActiveDetailModel(string name, string detail) : this(name)
        {
            Detail = detail;
        }

        #endregion

        #region Properties

        public bool Active { get; set; }

        #endregion
    }
}
