// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Core.Domain;
using Prolix.Core.Logic;
using Marketplace.Data;
using Marketplace.Domain.Security;

namespace Marketplace.Logic
{
    /// <summary>
    /// Ready-only repository bound to App Data Context
    /// </summary>
    /// <typeparam name="ModelType">Model Type</typeparam>
    public abstract class UpdatableService<ModelType> : UpdatableService<ModelType, IDataContext>
        where ModelType : class, IIdentifiable, IActivable
    {
        #region Constructors

        public UpdatableService(IDataContext context) : base(context)
        {
        }

        public UpdatableService(IDataContext context, SecurityContext security) : this(context)
        {
            Security = security;
        }

        #endregion

        #region Properties

        public SecurityContext Security { get; }

        #endregion
    }
}
