// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Data;
using Prolix.Core.Logic;
using Prolix.Core.Domain;
using Marketplace.Domain.Security;

namespace Marketplace.Logic
{
    /// <summary>
    /// Ready-only repository bound to App Data Context
    /// </summary>
    /// <typeparam name="ModelType">Model Type</typeparam>
    public abstract class RepositoryService<ModelType> : RepositoryService<ModelType, IDataContext>
        where ModelType : class, IIdentifiable
    {
        #region Constructors

        public RepositoryService(IDataContext context) : base(context)
        {
        }

        public RepositoryService(IDataContext context, SecurityContext security) : this(context)
        {
            Security = security;
        }

        #endregion

        #region Properties

        public SecurityContext Security { get; }

        #endregion
    }
}
