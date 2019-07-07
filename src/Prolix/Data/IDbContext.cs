// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

using Prolix.Ioc;

namespace Prolix.Data
{
    /// <summary>
    /// Represents a technology agnostic database context.
    /// </summary>
    public interface IDbContext : IDisposable, IContext
    {
        /// <summary>
        /// Returns a generic set of database entities. 
        /// All operations using this method results in database operations (after callling SaveChanges method).
        /// </summary>
        /// <typeparam name="T">The entity model</typeparam>
        /// <returns>A generic entity set</returns>
        IEntitySet<T> Set<T>() where T : class;

        /// <summary>
        /// Save all changes in all entity sets.
        /// </summary>
        /// <returns>The number of affected records</returns>
        Task<int> SaveChanges();

        /// <summary>
        /// Save all changes in all entity sets (async)
        /// </summary>
        /// <returns>The number of affected records</returns>
        Task<int> SaveChanges(CancellationToken cancellationToken);

        /// <summary>
        /// Start a new transaction
        /// </summary>
        void Start();

        /// <summary>
        /// Rollback the current transaction
        /// </summary>
        void Rollback();

        /// <summary>
        /// Confirm the current transaction
        /// </summary>
        void Commit();
    }
}
