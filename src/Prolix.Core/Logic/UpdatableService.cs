// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

using Prolix.Core.Data;
using Prolix.Core.Domain;
using Prolix.Core.Ioc;

namespace Prolix.Core.Logic
{
    /// <summary>
    /// Business service for repository with numeric Id.
    /// This service works with <seealso cref="IDbContext"/> from the data layer. 
    /// Most of times the database context is managed by an Ioc container, implemented from <see cref="Resolver" />
    /// </summary>
    /// <typeparam name="ModelType">The model type</typeparam>
    /// <typeparam name="ContextType">The daabase context type</typeparam>
    public abstract class UpdatableService<ModelType, ContextType> : UpdatableService<ModelType, int, ContextType>
        where ModelType : class, IIdentifiable, IActivable
        where ContextType : class, IDbContext
    {
        public UpdatableService(ContextType context) : base(context)
        {
        }
    }

    /// <summary>
    /// Business service for repository with generic Id.
    /// This service works with <seealso cref="IDbContext"/> from the data layer. 
    /// Most of times the database context is managed by an Ioc container, implemented from <see cref="Resolver" />
    /// </summary>
    /// <typeparam name="ModelType">The model type</typeparam>
    /// <typeparam name="KeyType">The model Id type</typeparam>
    /// <typeparam name="ContextType">The daabase context type</typeparam>
    public abstract class UpdatableService<ModelType, KeyType, ContextType> : RepositoryService<ModelType, KeyType, ContextType>, IUpdatableService<ModelType, KeyType>
        where ModelType : class, IIdentifiable<KeyType>
        where KeyType : struct, IComparable<KeyType>, IEquatable<KeyType>
        where ContextType : class, IDbContext
    {
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="UpdatableService{ModelType, KeyType, ContextType}"/>
        /// </summary>
        /// <param name="context">The database context
        public UpdatableService(ContextType context) : base(context)
        {
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Adds a model to the database
        /// </summary>
        /// <param name="model">The model to be saved</param>
        async public virtual Task Add(ModelType model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            Set.Add(model);

            await Context.SaveChanges();
        }

        /// <summary>
        /// Updates a model
        /// </summary>
        /// <param name="model">The model to be saved</param>
        /// <returns>True if data has been changed in the database.</returns>
        async public virtual Task<bool> Update(ModelType model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            var existing = Get(model.Id);

            if (existing == null)
                throw new ArgumentOutOfRangeException("A entidade nÃ£o existe no banco de dados.");

            Set.Update(model, existing);

            int affected = await Context.SaveChanges();
            return affected > 0;
        }

        /// <summary>
        /// Deletes a model from the database
        /// </summary>
        /// <param name="id">The Id of the model to be saved</param>
        /// <returns>True if data has been deleted in the database.</returns>
        async public virtual Task<bool> Delete(KeyType id)
        {
            var model = Get(id);

            return await Delete(model);
        }

        /// <summary>
        /// Deletes a model from the database
        /// </summary>
        /// <param name="model">The model to be saved</param>
        /// <returns>True if data has been deleted in the database.</returns>
        async public virtual Task<bool> Delete(ModelType model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            Set.Remove(model);

            int affected = await Context.SaveChanges();
            return affected > 0;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Validates an entity againt it's descriptor
        /// </summary>
        /// <param name="entity">The entity model</param>
        /// <param name="errorMessage">The error message to be displayed</param>
        /// <returns>True if no broken rules were found.</returns>
        protected bool Validate(ModelType entity, string errorMessage = null)
        {
            // Create a RuleValidation object
            RuleValidation rule = Validate<ModelType>(entity);

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
        /// <typeparam name="ChildType">The Model Model type</typeparam>
        /// <param name="entity">The entity model</param>
        /// <param name="errorMessage">The error message to be displayed</param>
        /// <returns>The resulting RuleValidation object</returns>
        protected RuleValidation Validate<ChildType>(ChildType entity, string errorMessage = null)
            where ChildType : class
        {
            RuleValidation result = new RuleValidation();

            ModelDescriptor<ChildType> descriptor = DescriptorManager.Get<ChildType>();

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
