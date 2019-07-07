// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Prolix.Logic
{
    /// <summary>
    /// Manages model business validation and metadata
    /// </summary>
    /// <typeparam name="TM">The model type</typeparam>
    public abstract class ModelDescriptor<TM> : IModelDescriptor
        where TM : class
    {
        #region Properties

        /// <summary>
        /// Model name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Descriptor fields. Each field has its rules.
        /// </summary>
        public IList<ModelDescriptorField<TM>> Fields { get; } = new List<ModelDescriptorField<TM>>();

        /// <summary>
        /// Descriptor rules.
        /// </summary>
        public IList<ModelDescriptorRule<TM>> Rules { get; } = new List<ModelDescriptorRule<TM>>();

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
        public ModelDescriptorField<TM, TP> Field<TP>(Expression<Func<TM, TP>> propertyExpression)
        {
            var item = new ModelDescriptorField<TM, TP>(propertyExpression);
            Fields.Add(item);
            return item;
        }

        /// <summary>
        /// Adds a descriptor rule.
        /// </summary>
        /// <param name="condition">The business rule condition.</param>
        /// <param name="message">The error message.</param>
        /// <returns>The descriptor</returns>
        public ModelDescriptor<TM> Validate(Expression<Func<TM, bool>> condition, string message)
        {
            var item = new ModelDescriptorRule<TM>(condition, message);
            Rules.Add(item);
            return this;
        }

        /// <summary>
		/// Validates a model and build a <see cref="RuleValidation"/> object
		/// </summary>
		/// <param name="model">The model model to be validated</param>
		/// <returns>The <see cref="RuleValidation"/> object with all error messages</returns>
		public virtual RuleValidation Build(TM model)
        {
            RuleValidation result = new RuleValidation();

            foreach (ModelDescriptorRule<TM> rule in Rules)
            {
                Func<TM, bool> validator = rule.Condition.Compile();
                bool success = model != null && validator(model);

                if (!success)
                    result.Add("", rule.Message);
            }

            foreach (ModelDescriptorField<TM> field in Fields)
            {
                foreach (ModelDescriptorRule<TM> rule in field.Rules)
                {
                    Func<TM, bool> validator = rule.Condition.Compile();
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
        public virtual IList<ModelAudit> Audit(TM current, TM data)
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
