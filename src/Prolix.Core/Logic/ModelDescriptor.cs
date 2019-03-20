// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Prolix.Core.Logic
{
    /// <summary>
    /// Manages model business validation and metadata
    /// </summary>
    /// <typeparam name="ModelType">The model type</typeparam>
    public abstract class ModelDescriptor<ModelType> : IModelDescriptor
        where ModelType : class
    {
        #region Properties

        /// <summary>
        /// Nome da entidade
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Descriptor fields. Each field has its rules.
        /// </summary>
        public IList<ModelDescriptorField<ModelType>> Fields { get; } = new List<ModelDescriptorField<ModelType>>();

        /// <summary>
        /// Descriptor rules.
        /// </summary>
        public IList<ModelDescriptorRule<ModelType>> Rules { get; } = new List<ModelDescriptorRule<ModelType>>();

        #endregion

        #region Methods

        /// <summary>
        /// Sets the model description.
        /// </summary>
        /// <param name="name">The model description</param>
        public void Model(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Adds a descriptor field.
        /// </summary>
        /// <param name="propertyExpression">The field expression</param>
        /// <returns>The created fields descriptor.</returns>
        public ModelDescriptorField<ModelType, FieldType> Field<FieldType>(Expression<Func<ModelType, FieldType>> propertyExpression)
        {
            var item = new ModelDescriptorField<ModelType, FieldType>(propertyExpression);
            Fields.Add(item);
            return item;
        }

        /// <summary>
        /// Adds a descriptor rule.
        /// </summary>
        /// <param name="condition">The business rule condition.</param>
        /// <param name="message">The error message.</param>
        /// <returns>The descriptor</returns>
        public ModelDescriptor<ModelType> Validate(Expression<Func<ModelType, bool>> condition, string message)
        {
            var item = new ModelDescriptorRule<ModelType>(condition, message);
            Rules.Add(item);
            return this;
        }

        /// <summary>
		/// Validates a model and build a <see cref="RuleValidation"/> object
		/// </summary>
		/// <param name="model">The model model to be validated</param>
		/// <returns>The <see cref="RuleValidation"/> object with all error messages</returns>
		public virtual RuleValidation Build(ModelType model)
        {
            RuleValidation result = new RuleValidation();

            foreach (ModelDescriptorRule<ModelType> rule in Rules)
            {
                Func<ModelType, bool> validator = rule.Condition.Compile();
                bool success = model != null && validator(model);

                if (!success)
                    result.Add("", rule.Message);
            }

            foreach (ModelDescriptorField<ModelType> field in Fields)
            {
                foreach (ModelDescriptorRule<ModelType> rule in field.Rules)
                {
                    Func<ModelType, bool> validator = rule.Condition.Compile();
                    bool success = model != null && validator(model);

                    if (!success)
                        result.Add(field.Name, rule.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Builds a list of audit changes between two models.
        /// </summary>
        /// <param name="current">The current model</param>
        /// /// <param name="data">The new model</param>
        /// <returns>All audit changes.</returns>
        public virtual IList<ModelAudit> Audit(ModelType current, ModelType data)
        {
            var result = new List<ModelAudit>();

            foreach (var field in Fields)
            {
                object newValue = string.Empty;
                object oldValue = string.Empty;

                if (data != null)
                    newValue = field.GetValue(data);
                if (current != null)
                    oldValue = field.GetValue(current);

                var item = new ModelAudit(field.Text, newValue, oldValue);

                result.Add(item);
            }

            return result;
        }

        #endregion
    }
}
