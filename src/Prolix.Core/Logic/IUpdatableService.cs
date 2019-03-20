// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Prolix.Core.Domain;

namespace Prolix.Core.Logic
{
    /// <summary>
    /// Business service for repository with numeric Id.
    /// This service works with <seealso cref="IDbContext"/> from the data layer. 
    /// Most of times the database context is managed by an Ioc container, implemented from <see cref="Ioc.IResolver" />
    /// </summary>
    /// <typeparam name="ModelType">The model type</typeparam>
    /// <typeparam name="ContextType">The daabase context type</typeparam>
    public interface IUpdatableService<ModelType> : IUpdatableService<ModelType, int>
        where ModelType : class, IIdentifiable, IActivable
    {
    }

    public interface IUpdatableService<ModelType, KeyType> : IRepositoryService<ModelType, KeyType>
        where ModelType : class, IIdentifiable<KeyType>
        where KeyType : struct, IComparable<KeyType>, IEquatable<KeyType>
    {
        /// <summary>
        /// Adds a model to the database
        /// </summary>
        /// <param name="model">The model to be saved</param>
        Task Add(ModelType entity);

        /// <summary>
        /// Updates a model
        /// </summary>
        /// <param name="model">The model to be saved</param>
        /// <returns>True if data has been changed in the database.</returns>
        Task<bool> Update(ModelType entity);

        /// <summary>
        /// Deletes a model from the database
        /// </summary>
        /// <param name="id">The Id of the model to be saved</param>
        /// <returns>True if data has been deleted in the database.</returns>
        Task<bool> Delete(KeyType id);

        /// <summary>
        /// Deletes a model from the database
        /// </summary>
        /// <param name="model">The model to be saved</param>
        /// <returns>True if data has been deleted in the database.</returns>
        Task<bool> Delete(ModelType entity);        
    }
}
