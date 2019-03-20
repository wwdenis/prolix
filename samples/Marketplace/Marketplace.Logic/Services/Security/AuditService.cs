// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Marketplace.Data;
using Marketplace.Domain.Security;
using Marketplace.Domain.Models.Security;
using Marketplace.Logic.Contracts.Security;

using Prolix.Core.Domain;
using Prolix.Core.Logic;

namespace Marketplace.Logic.Services.Security
{
    public class AuditService : UpdatableService<AuditLog>, IAuditService
    {
        #region Constructors

        public AuditService(IDataContext context, SecurityContext security) : base(context, security)
        {
        }

        #endregion

        #region Public Methods

        async public override Task Add(AuditLog model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (string.IsNullOrWhiteSpace(model.Detail))
                throw new ArgumentOutOfRangeException("description");

            model.Active = true;
            
            await base.Add(model);
        }

        public AuditLog Create<ModelType>(AuditType type, ModelType model = null, ModelType data = null)
            where ModelType : Model
        {
            return Create(type, string.Empty, model, data);
        }

        public AuditLog Create<ModelType>(AuditType type, string detail, ModelType current = null, ModelType data = null)
            where ModelType : Model
        {
            Feature feature = Security?.Feature;

            //if (feature == null)
            //    throw new ApplicationException("Feature not found ");

            var userId = Security?.User?.Id;
            
            var descriptor = DescriptorManager.Get<ModelType>();
            var audit = DescriptorManager.Audit(current, data);

            var entityName = descriptor?.Name;
            var modelId = data?.Id ?? current?.Id;
            var featureId = feature?.Id;

            var changes = from i in audit
                          select new AuditChange(i.Name, i.NewValue, i.OldValue);

            if (string.IsNullOrWhiteSpace(detail))
                detail = "Audit";

            var log = new AuditLog
            {
                UserId = userId,
                FeatureId = featureId,
                Detail = detail,
                ModelId = modelId,
                Changes = changes?.ToList()
            };

            return log;
        }

        #endregion
    }
}
