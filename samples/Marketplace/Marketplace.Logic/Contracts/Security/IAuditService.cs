// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Security;
using Prolix.Core.Logic;
using Prolix.Core.Domain;

namespace Marketplace.Logic.Contracts.Security
{
    public interface IAuditService : IRepositoryService<AuditLog>
    {
        /// <summary>
        /// Creates an log based on model descriptor
        /// </summary>
        /// <typeparam name="ModelType">The model type</typeparam>
        /// <param name="type">The operation type</param>
        /// <param name="current">The affected model</param>
        /// <param name="data">The new data (for editing)</param>
        /// <returns>An AutitLog instance</returns>
        AuditLog Create<ModelType>(AuditType type, ModelType current = null, ModelType data = null)
            where ModelType : Model;

        /// <summary>
        /// Creates an log based on model descriptor
        /// </summary>
        /// <typeparam name="ModelType">The model type</typeparam>
        /// <param name="type">The operation type</param>
        /// <param name="detail">The lof detail</param>
        /// <param name="current">The affected model</param>
        /// <param name="data">The new data (for editing)</param>
        /// <returns>An AutitLog instance</returns>
        AuditLog Create<ModelType>(AuditType type, string detail, ModelType current = null, ModelType data = null)
            where ModelType : Model;
    }
}
