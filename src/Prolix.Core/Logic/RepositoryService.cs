// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;

using Prolix.Core.Data;
using Prolix.Core.Domain;

namespace Prolix.Core.Logic
{
    /// <summary>
    /// Business service for read-only repository with numeric Id.
    /// This service works with <seealso cref="IDbContext"/> from the data layer. 
    /// Most of times the database context is managed by an Ioc container, implemented from <see cref="Ioc.IResolver" />
    /// </summary>
    /// <typeparam name="ModelType">The model type</typeparam>
    /// <typeparam name="ContextType">The daabase context type</typeparam>
    public abstract class RepositoryService<ModelType, ContextType> : RepositoryService<ModelType, int, ContextType>
        where ModelType : class, IIdentifiable
        where ContextType : class, IDbContext
    {
        public RepositoryService(ContextType context) : base(context)
        {
        }
    }

    /// <summary>
    /// Business service for read-only repository with generic Id.
    /// This service works with <seealso cref="IDbContext"/> from the data layer. 
    /// Most of times the database context is managed by an Ioc container, implemented from <see cref="Ioc.IResolver" />
    /// </summary>
    /// <typeparam name="ModelType">The model type</typeparam>
    /// <typeparam name="KeyType">The model Id type</typeparam>
    /// <typeparam name="ContextType">The daabase context type</typeparam>
    public abstract class RepositoryService<ModelType, KeyType, ContextType> : IRepositoryService<ModelType, KeyType> 
        where ModelType : class, IIdentifiable<KeyType>
        where KeyType : struct, IComparable<KeyType>, IEquatable<KeyType>
        where ContextType : class, IDbContext
    {
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="RepositoryService{ModelType, KeyType, ContextType}"/>
        /// </summary>
        /// <param name="context">The database context</param>
        public RepositoryService(ContextType context)
        {
            Context = context;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The database context
        /// </summary>
        protected ContextType Context { get; }

        /// <summary>
        /// The model entity set
        /// </summary>
        protected IEntitySet<ModelType> Set => Context?.Set<ModelType>();

        /// <summary>
        /// The rule validation instance
        /// </summary>
        public RuleValidation Rule { get; } = new RuleValidation();

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the first ocurrence of the specificed criteria.
        /// </summary>
        /// <param name="criteria">Criteria expression.</param>
        /// <returns>The first ocurrence of the model, or null, if the criteria doesn't match.</returns>
        public virtual ModelType Get(Expression<Func<ModelType, bool>> criteria)
        {
            return Set.FirstOrDefault(criteria);
        }

        /// <summary>
        /// Gets a model based on its Id.
        /// </summary>
        /// <param name="id">The model Id</param>
        /// <returns>The first ocurrence of the model, or null, if the model doesn't exist.</returns>
        public virtual ModelType Get(KeyType id)
        {
            return Set.FirstOrDefault(i => i.Id.Equals(id));
        }

        /// <summary>
        /// Gets all ocurrences of a model from the database.
        /// </summary>
        public virtual IQueryable<ModelType> List()
        {
            return Set;
        }

        /// <summary>
        /// Gets all ocurrences of the specificed criteria.
        /// </summary>
        /// <param name="criteria">Criteria expression.</param>
        /// <returns>All ocurrences of the model, or a empty list, if the criteria doesn't match.</returns>
        public virtual IQueryable<ModelType> Find(Expression<Func<ModelType, bool>> criteria)
        {
            return Set.Where(criteria);
        }

        /// <summary>
        /// Checks if a model exists in the database based on its Id.
        /// </summary>
        /// <param name="id">The model Id</param>
        /// <returns>TRUE if the model exists. Otherwise FALSE.</returns>
        public virtual bool Exists(KeyType id)
        {
            return Exists(i => id.Equals(i.Id));
        }

        /// <summary>
        /// Checks if a model exists in the database based on given criteria.
        /// </summary>
        /// <param name="criteria">Criteria expression.</param>
        /// <returns>TRUE if the model exists. Otherwise FALSE.</returns>
        public virtual bool Exists(Expression<Func<ModelType, bool>> criteria)
        {
            return Set.Any(criteria);
        }

        /// <summary>
        /// Checks if other model exists in the database based on its Id.
        /// This method is useful when trying to query other models without instantiating other services.
        /// </summary>
        /// <param name="id">The model Id</param>
        /// <typeparam name="ReferenceType">The target model type</typeparam>
        /// <returns>TRUE if the model exists. Otherwise FALSE.</returns>
        public bool Exists<ReferenceType>(KeyType? id)
            where ReferenceType : class, IIdentifiable
        {
            if (id == null || !id.HasValue)
                return false;

            var value = id.Value;
            var set = Context.Set<ReferenceType>();

            if (set == null)
                return false;

            return set.Any(i => value.Equals(i.Id));
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Check if there are errors on <see cref="RuleValidation" />. 
        /// If yes, throws a <see cref="RuleException"/>
        /// </summary>
        /// <param name="message">Parent error message.</param>
        protected void CheckRule(string message)
        {
            Rule?.Check(message);
        }

        /// <summary>
        /// Check if there are errors on <see cref="RuleValidation" />. 
        /// If yes, throws a <see cref="RuleException"/>
        /// </summary>
        protected void CheckRule()
        {
            CheckRule(RuleValidation.DefaultMessage);
        }

        #endregion
    }
}
 
