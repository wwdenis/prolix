// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;

using Wwa.Core.Collections;
using Wwa.Core.Extensions.Collections;
using Wwa.Core.Extensions.Reflection;

namespace Wwa.Core.Logic
{
    /// <summary>
    /// Manages descriptors
    /// </summary>
    public sealed class DescriptorManager
    {
        /// <summary>
        /// All mapped descriptors
        /// </summary>
        static IDictionary<Type, Type> MappedDescriptors { get; } = new WeakDictionary<Type, Type>();

        /// <summary>
        /// Add descriptor mappings (Model, Descritor) to the global descriptor cache.
        /// </summary>
        /// <param name="mappings">The descriptor mappings needed to be added</param>
        public static void Configure(IDictionary<Type, Type> mappings)
        {
            MappedDescriptors.AddRange(mappings);
        }

        /// <summary>
        /// Gets an descriptor for a model, from <seealso cref="DescribeAttribute"/> or from the global cache.
        /// </summary>
        /// <typeparam name="ModelType">The model type</typeparam>
        /// <returns>The descriptor instance</returns>
        public static ModelDescriptor<ModelType> Get<ModelType>()
            where ModelType : class
        {
            Type descriptorType = null;

            var modelType = typeof(ModelType);
            var attr = modelType.GetAttribute<DescribeAttribute>();

            descriptorType = attr?.DescriptorType;

            if (descriptorType == null && MappedDescriptors != null)
                descriptorType = MappedDescriptors[modelType];

            if (descriptorType == null)
                return null;

            var descriptor = descriptorType.Instantiate<ModelDescriptor<ModelType>>();

            return descriptor;
        }

        /// <summary>
        /// List changes between two models, and the field names listed in the model descriptor.
        /// </summary>
        /// <typeparam name="ModelType">The model type</typeparam>
        /// <param name="old">The old data</param>
        /// <param name="@new">The new data</param>
        /// <returns>A list of <seealso cref="ModelAudit"/></returns>
        public static IList<ModelAudit> Audit<ModelType>(ModelType old, ModelType @new)
            where ModelType : class
        {
            var descriptor = Get<ModelType>();

            if (descriptor == null)
                return new List<ModelAudit>();

            var audit = descriptor.Audit(old, @new);
            return audit;
        }

        /// <summary>
        /// Validates business rules against the model descriptor.
        /// </summary>
        /// <typeparam name="ModelType">The model type</typeparam>
        /// <param name="model">The model instance</param>
        /// <param name="message">The parent validation message</param>
        /// <returns>A <seealso cref="RuleValidation"/> instance with the validation result.</returns>
        public static RuleValidation Validate<ModelType>(ModelType model, string message = null)
            where ModelType : class
        {
            var descriptor = Get<ModelType>();

            if (descriptor == null)
                return new RuleValidation(message);

            var rule = descriptor.Build(model);

            if (!string.IsNullOrWhiteSpace(message))
                rule.Message = message;

            return rule;
        }
    }
}
