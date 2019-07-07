// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Prolix.Extensions.Expressions;

namespace Prolix.Logic
{
    /// <summary>
    /// Base Field descriptor
    /// </summary>
    /// <typeparam name="T">Model type</typeparam>
    public abstract class ModelDescriptorField<T>
        where T : class
    {
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ModelDescriptor{T}" /> based on a expression
        /// </summary>
        /// <param name="propertyExpression">The property expression</param>
        public ModelDescriptorField(LambdaExpression propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            // Hack EF object expressions
            Property = propertyExpression.Normalize();
        }

        #endregion

        #region Private Properties

        LambdaExpression Property { get; }

        /// <summary>
        /// Refleced property info
        /// </summary>
        PropertyInfo Info => Property?.GetInfo();

        /// <summary>
        /// Field type
        /// </summary>
        Type Type => Info?.PropertyType;

        #endregion

        #region Public Properties

        public IList<ModelDescriptorRule<T>> Rules { get; } = new List<ModelDescriptorRule<T>>();

        /// <summary>
        /// Property name
        /// </summary>
        public string Name => Info?.Name ?? string.Empty;

        /// <summary>
        /// Property description
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the property value
        /// </summary>
        /// <param name="entity">The model</param>
        /// <returns>The property value</returns>
        public object GetValue(T entity)
        {
            var get = Property.Compile();
            return get.DynamicInvoke(entity);
        }

        #endregion
    }

    /// <summary>
    /// Field descriptor
    /// </summary>
    /// <typeparam name="TM">Model type</typeparam>
    /// <typeparam name="TP">The property type</typeparam>
    public sealed class ModelDescriptorField<TM, TP> : ModelDescriptorField<TM>
        where TM : class
    {
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ModelDescriptor{T}" /> based on a expression
        /// </summary>
        /// <param name="propertyExpression">The property expression</param>
        public ModelDescriptorField(LambdaExpression propertyExpression) : base(propertyExpression)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the field description
        /// </summary>
        /// <param name="caption">The field description</param>
        /// <returns>The descriptor</returns>
        public ModelDescriptorField<TM, TP> Caption(string caption)
        {
            Text = caption;
            return this;
        }

        /// <summary>
        /// Sets the required rule validation
        /// </summary>
        /// <param name="message">The error message when the condition is not met.</param>
        /// <returns>The descriptor</returns>
        public ModelDescriptorField<TM, TP> Required(string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
                message = string.Format("{0} is required", Text);

            message = message.Replace("  ", " ");

            Validate(i => !IsEmpty(i), message);

            return this;
        }

        /// <summary>
        /// Sets the maximum length rule validation
        /// </summary>
        /// <param name="maxLength">The maximum length</param>
        /// <param name="message">The error message when the condition is not met.</param>
        /// <returns>The descriptor</returns>
        public ModelDescriptorField<TM, TP> MaxLength(int maxLength, string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
                message = string.Format("{0} maximum length is {1} characters", Text, maxLength);

            message = message.Replace("  ", " ");

            Validate(i => IsLengthLessThan(i, maxLength), message);
            return this;
        }

        /// <summary>
        /// Sets the minimum length rule validation
        /// </summary>
        /// <param name="minLength">The minimum length</param>
        /// <param name="message">The error message when the condition is not met.</param>
        /// <returns>The descriptor</returns>
        public ModelDescriptorField<TM, TP> MinLength(int minLength, string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
                message = string.Format("{0} minimum length is {1} characters", Text, minLength);

            message = message.Replace("  ", " ");

            Validate(i => IsLengthGreaterThan(i, minLength), message);
            return this;
        }

        /// <summary>
        /// Sets the allowed values for the field
        /// </summary>
        /// <param name="values">The value list</param>
        /// <returns>The descriptor</returns>
        public ModelDescriptorField<TM, TP> Contains(TP[] values, string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                var list = string.Join(", ", values);
                message = string.Format("The allowed values are: {0}", list);
            }

            message = message.Replace("  ", " ");

            Validate(i => SearchArray(i, values), message);
            return this;
        }

        /// <summary>
        /// Sets the exact value for the field
        /// </summary>
        /// <param name="value">The allowed value</param>
        /// <returns>The descriptor</returns>
        public ModelDescriptorField<TM, TP> Exact(TP value, string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
                message = string.Format("The allowed value is: {0}", value);
            
            message = message.Replace("  ", " ");

            Validate(i => IsEqual(i, value), message);
            return this;
        }

        /// <summary>
        /// Sets the minimum value for the field
        /// </summary>
        /// <param name="value">The minimum value</param>
        /// <returns>The descriptor</returns>
        public ModelDescriptorField<TM, TP> Minimum(TP value, string message = "", params string[] args)
        {
            if (string.IsNullOrWhiteSpace(message))
                message = string.Format("The minimum value is {0}", value);

            message = message.Replace("  ", " ");

            Validate(i => IsGreater(i, value) || IsEqual(i, value), message);
            return this;
        }

        /// <summary>
        /// Sets the maximum value for the field
        /// </summary>
        /// <param name="value">The maximum value</param>
        /// <returns>The descriptor</returns>
        public ModelDescriptorField<TM, TP> Maximum(TP value, string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
                message = string.Format("The maximum value is {0}", value);

            message = message.Replace("  ", " ");

            Validate(i => IsLess(i, value) || IsEqual(i, value), message);
            return this;
        }

        /// <summary>
        /// Sets the maximum limit for the field
        /// </summary>
        /// <param name="value">The specified value</param>
        /// <returns>The descriptor</returns>
        public ModelDescriptorField<TM, TP> GreaterThan(TP value, string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
                message = string.Format("The value must be greater than {0}", value);

            message = message.Replace("  ", " ");

            Validate(i => IsGreater(i, value), message);
            return this;
        }

        /// <summary>
        /// Sets the minimum limit for the field
        /// </summary>
        /// <param name="value">The specified value</param>
        /// <returns>The descriptor</returns>
        public ModelDescriptorField<TM, TP> LessThan(TP value, string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
                message = string.Format("The value must be less than {0}", value);

            message = message.Replace("  ", " ");

            Validate(i => IsLess(i, value), message);
            return this;
        }

        /// <summary>
        /// Sets the allowed range for the field
        /// </summary>
        /// <param name="min">Minimun value</param>
        /// <param name="max">Maximun value</param>
        /// <returns>The descriptor</returns>
        public ModelDescriptorField<TM, TP> Range(TP min, TP max, string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
                message = string.Format("The allowed range is between {0} and {1}", min, max);

            message = message.Replace("  ", " ");

            Validate(i => IsLessOrEqual(i, max) && IsLessOrEqual(i, max), message);
            return this;
        }

        /// <summary>
        /// Adds a custom validation rule
        /// </summary>
        /// <param name="condition">The validation expression</param>
        /// <param name="message">The error message</param>
        /// <returns>The descriptor</returns>
        public void Validate(Expression<Func<TM, bool>> condition, string message = "")
        {
            var item = new ModelDescriptorRule<TM>(condition, message);
            Rules.Add(item);
        }

        #endregion

        #region Private Methods

        TP TryGetValue(TM entity)
        {
            if (entity == null)
                return default(TP);

            var obj = GetValue(entity);

            if (obj is TP)
                return (TP)obj;

            return default(TP);
        }

        bool SearchArray(TM entity, TP[] values)
        {
            if (values == null || !values.Any())
                return false;

            var value = TryGetValue(entity);
            var found = values.Contains(value);
            return found;
        }

        int? Compare(TM entity, TP value)
        {
            var current = GetValue(entity);
            var compare = current as IComparable;

            if (current == null || compare == null)
                return null;
            
            var result = compare.CompareTo(value);

            return result;
        }

        bool IsEqual(TM entity, TP value)
        {
            var compare = Compare(entity, value);
            return compare != null && compare == 0;
        }

        bool IsLess(TM entity, TP value)
        {
            var compare = Compare(entity, value);
            return compare != null && compare < 0;
        }

        bool IsGreater(TM entity, TP value)
        {
            var compare = Compare(entity, value);
            return compare != null && compare > 0;
        }

        bool IsLessOrEqual(TM entity, TP value)
        {
            var compare = Compare(entity, value);
            return compare != null && (compare < 0 || compare == 0);
        }

        bool IsGreaterOrEqual(TM entity, TP value)
        {
            var compare = Compare(entity, value);
            return compare != null && (compare > 0 || compare == 0);
        }

        bool IsEmpty(TM entity)
        {
            var value = GetValue(entity);

            if (value == null)
                return true;

            if (value is string)
            {
                string parsed = string.Format("{0}", value);
                return string.IsNullOrWhiteSpace(parsed);
            }
            else if (value is DateTime || value is DateTime?)
            {
                DateTime parsed = (DateTime)value;
                return parsed == DateTime.MinValue;
            }

            return false;
        }

        bool IsLengthGreaterThan(TM entity, int minLength)
        {
            var value = GetValue(entity);

            if (value == null || !(value is string))
                return true;

            string parsed = string.Format("{0}", value);
            return parsed.Length >= minLength;
        }

        bool IsLengthLessThan(TM entity, int maxLength)
        {
            var value = GetValue(entity);

            if (value == null || !(value is string))
                return true;

            string parsed = string.Format("{0}", value);
            return parsed.Length <= maxLength;
        }

        #endregion
    }
}
