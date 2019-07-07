// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Prolix.Data;
using Prolix.Domain;

namespace Prolix.Logic
{
    /// <summary>
    /// Business service for read-only repository with numeric Id.
    /// This service works with <seealso cref="IDbContext"/> from the data layer. 
    /// Most of times the database context is managed by an Ioc container, implemented from <see cref="Resolver" />
    /// </summary>
    /// <typeparam name="TM">The model type</typeparam>
    /// <typeparam name="TC">The daabase context type</typeparam>
    public abstract class RepositoryService<TM, TC> : RepositoryService<TM, int, TC>
        where TM : class, IIdentifiable
        where TC : class, IDbContext
    {
        public RepositoryService(TC context) : base(context)
        {
        }
    }

    /// <summary>
    /// Business service for read-only repository with generic Id.
    /// This service works with <seealso cref="IDbContext"/> from the data layer. 
    /// Most of times the database context is managed by an Ioc container, implemented from <see cref="Resolver" />
    /// </summary>
    /// <typeparam name="TM">The model type</typeparam>
    /// <typeparam name="TK">The model Id type</typeparam>
    /// <typeparam name="TC">The database context type</typeparam>
    public abstract class RepositoryService<TM, TK, TC> : IRepositoryService<TM, TK> 
        where TM : class, IIdentifiable<TK>
        where TK : struct, IComparable<TK>, IEquatable<TK>
        where TC : class, IDbContext
    {
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="RepositoryService{TM, TK, TC}"/>
        /// </summary>
        /// <param name="context">The database context</param>
        public RepositoryService(TC context)
        {
            Context = context;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The database context
        /// </summary>
        protected TC Context { get; }

        /// <summary>
        /// The model entity set
        /// </summary>
        protected IEntitySet<TM> Set => Context?.Set<TM>();

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
        public virtual TM Get(Expression<Func<TM, bool>> criteria)
        {
            return Set.FirstOrDefault(criteria);
        }

        /// <summary>
        /// Gets a model based on its Id.
        /// </summary>
        /// <param name="id">The model Id</param>
        /// <returns>The first ocurrence of the model, or null, if the model doesn't exist.</returns>
        public virtual TM Get(TK id)
        {
            return Set.FirstOrDefault(i => i.Id.Equals(id));
        }

        /// <summary>
        /// Gets all ocurrences of a model from the database.
        /// </summary>
        public virtual IQueryable<TM> List()
        {
            return Set;
        }

        /// <summary>
        /// Gets all ocurrences of the specificed criteria.
        /// </summary>
        /// <param name="criteria">Criteria expression.</param>
        /// <returns>All ocurrences of the model, or a empty list, if the criteria doesn't match.</returns>
        public virtual IQueryable<TM> Find(Expression<Func<TM, bool>> criteria)
        {
            return Set.Where(criteria);
        }

        /// <summary>
        /// Checks if a model exists in the database based on its Id.
        /// </summary>
        /// <param name="id">The model Id</param>
        /// <returns>TRUE if the model exists. Otherwise FALSE.</returns>
        public virtual bool Exists(TK id)
        {
            return Exists(i => id.Equals(i.Id));
        }

        /// <summary>
        /// Checks if a model exists in the database based on given criteria.
        /// </summary>
        /// <param name="criteria">Criteria expression.</param>
        /// <returns>TRUE if the model exists. Otherwise FALSE.</returns>
        public virtual bool Exists(Expression<Func<TM, bool>> criteria)
        {
            return Set.Any(criteria);
        }

        /// <summary>
        /// Checks if other model exists in the database based on its Id.
        /// This method is useful when trying to query other models without instantiating other services.
        /// </summary>
        /// <param name="id">The model Id</param>
        /// <typeparam name="T">The target model type</typeparam>
        /// <returns>TRUE if the model exists. Otherwise FALSE.</returns>
        public bool Exists<T>(TK? id)
            where T : class, IIdentifiable
        {
            if (id == null || !id.HasValue)
                return false;

            var value = id.Value;
            var set = Context.Set<T>();

            if (set == null)
                return false;

            return set.Any(i => value.Equals(i.Id));
        }

        /// <summary>
        /// Adds a model to the database
        /// </summary>
        /// <param name="model">The model to be saved</param>
        async public virtual Task Add(TM model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Set.Add(model);

            await Context.SaveChanges();
        }

        /// <summary>
        /// Updates a model
        /// </summary>
        /// <param name="model">The model to be saved</param>
        /// <returns>True if data has been changed in the database.</returns>
        async public virtual Task<bool> Update(TM model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var existing = Get(model.Id);

            if (existing == null)
                throw new ArgumentOutOfRangeException(nameof(model), "The model does not exists in the database.");

            Set.Update(model, existing);

            int affected = await Context.SaveChanges();
            return affected > 0;
        }

        /// <summary>
        /// Deletes a model from the database
        /// </summary>
        /// <param name="id">The Id of the model to be saved</param>
        /// <returns>True if data has been deleted in the database.</returns>
        async public virtual Task<bool> Delete(TK id)
        {
            var model = Get(id);

            return await Delete(model);
        }

        /// <summary>
        /// Deletes a model from the database
        /// </summary>
        /// <param name="model">The model to be saved</param>
        /// <returns>True if data has been deleted in the database.</returns>
        async public virtual Task<bool> Delete(TM model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Set.Remove(model);

            int affected = await Context.SaveChanges();
            return affected > 0;
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

        /// <summary>
        /// Validates an entity againt it's descriptor
        /// </summary>
        /// <param name="entity">The entity model</param>
        /// <param name="errorMessage">The error message to be displayed</param>
        /// <returns>True if no broken rules were found.</returns>
        protected bool Validate(TM entity, string errorMessage = null)
        {
            // Create a RuleValidation object
            RuleValidation rule = Validate<TM>(entity);

            Rule.Merge(rule);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                Rule.Check(errorMessage);
            }

            return !Rule.HasErrors();
        }

        /// <summary>
        /// Validates an entity againt it's descriptor
        /// </summary>
        /// <typeparam name="T">The Model Model type</typeparam>
        /// <param name="entity">The entity model</param>
        /// <param name="errorMessage">The error message to be displayed</param>
        /// <returns>The resulting RuleValidation object</returns>
        protected RuleValidation Validate<T>(T entity, string errorMessage = null)
            where T : class
        {
            RuleValidation result = new RuleValidation();

            ModelDescriptor<T> descriptor = DescriptorManager.Get<T>();

            if (descriptor == null)
                return result;

            // Create a RuleValidation object
            RuleValidation rule = descriptor.Build(entity);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                rule.Check(errorMessage);
            }

            return rule;
        }

        #endregion
    }
}
 
