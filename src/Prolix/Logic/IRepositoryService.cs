// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Prolix.Domain;
using Prolix.Ioc;

namespace Prolix.Logic
{
    /// <summary>
    /// Business service for repository with numeric Id.
    /// This service works with <seealso cref="IDbContext"/> from the data layer. 
    /// Most of times the database context is managed by an Ioc container, implemented from <see cref="Resolver" />
    /// </summary>
    /// <typeparam name="T">The model type</typeparam>
    public interface IRepositoryService<T> : IRepositoryService<T, int>
        where T : class, IIdentifiable<int>
    {
    }

    /// <summary>
    /// Business service for repository with generic Id.
    /// This service works with <seealso cref="IDbContext"/> from the data layer. 
    /// Most of times the database context is managed by an Ioc container, implemented from <see cref="Resolver" />
    /// </summary>
    /// <typeparam name="TM">The model type</typeparam>
    /// <typeparam name="TK">The model Id type</typeparam>
    public interface IRepositoryService<TM, TK> : IService
        where TM : class, IIdentifiable<TK>
        where TK : struct, IComparable<TK>, IEquatable<TK>
    {
        /// <summary>
        /// Gets the first ocurrence of the specificed criteria.
        /// </summary>
        /// <param name="criteria">Criteria expression.</param>
        /// <returns>The first ocurrence of the model, or null, if the criteria doesn't match.</returns>
        TM Get(Expression<Func<TM, bool>> criteria);

        /// <summary>
        /// Gets a model based on its Id.
        /// </summary>
        /// <param name="id">The model Id</param>
        /// <returns>The first ocurrence of the model, or null, if the model doesn't exist.</returns>
        TM Get(TK id);

        /// <summary>
        /// Gets all ocurrences of a model from the database.
        /// </summary>
        IQueryable<TM> List();

        /// <summary>
        /// Gets all ocurrences of the specificed criteria.
        /// </summary>
        /// <param name="criteria">Criteria expression.</param>
        /// <returns>All ocurrences of the model, or a empty list, if the criteria doesn't match.</returns>
        IQueryable<TM> Find(Expression<Func<TM, bool>> criteria);
        
        /// <summary>
        /// Checks if a model exists in the database based on its Id.
        /// </summary>
        /// <param name="id">The model Id</param>
        /// <returns>TRUE if the model exists. Otherwise FALSE.</returns>
        bool Exists(TK id);

        /// <summary>
        /// Checks if a model exists in the database based on given criteria.
        /// </summary>
        /// <param name="criteria">Criteria expression.</param>
        /// <returns>TRUE if the model exists. Otherwise FALSE.</returns>
        bool Exists(Expression<Func<TM, bool>> criteria);

        /// <summary>
        /// Adds a model to the database
        /// </summary>
        /// <param name="model">The model to be saved</param>
        Task Add(TM entity);

        /// <summary>
        /// Updates a model
        /// </summary>
        /// <param name="model">The model to be saved</param>
        /// <returns>True if data has been changed in the database.</returns>
        Task<bool> Update(TM entity);

        /// <summary>
        /// Deletes a model from the database
        /// </summary>
        /// <param name="id">The Id of the model to be saved</param>
        /// <returns>True if data has been deleted in the database.</returns>
        Task<bool> Delete(TK id);

        /// <summary>
        /// Deletes a model from the database
        /// </summary>
        /// <param name="model">The model to be saved</param>
        /// <returns>True if data has been deleted in the database.</returns>
        Task<bool> Delete(TM entity);
    }
}
