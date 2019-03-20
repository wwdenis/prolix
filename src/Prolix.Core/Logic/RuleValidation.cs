// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Prolix.Core.Logic
{
    /// <summary>
    /// Business validation container.
    /// </summary>
    public sealed class RuleValidation
    {
        #region Constants

        public const string DefaultMessage = "One or more conditions are not met.";

        #endregion

        #region Constructors

        public RuleValidation() : this(DefaultMessage)
        {
        }

        public RuleValidation(string message)
        {
            Message = message;
        }

        public RuleValidation(string message, params RuleError[] errors) : this(message)
        {
            Errors.AddRange(errors);
        }

        public RuleValidation(string message, Dictionary<string, string> errors) : this(message)
        {
            if (errors?.Any() ?? false)
            {
                var result = from i in errors
                             select new RuleError(i.Key, i.Value);

                Errors.AddRange(result);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Parent error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Validation field errors
        /// </summary>
        public List<RuleError> Errors { get; } = new List<RuleError>();

        /// <summary>
        /// Returns TRUE if there are associated errors
        /// </summary>
        public bool HasErrors()
        {
            return Errors != null && Errors.Any();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an rule validation error to the <see cref="Errors"/> collection
        /// </summary>
        /// <param name="field">Field name</param>
        /// <param name="message">Error message</param>
        /// <param name="args">Error message template arguments <see cref="string.Format"/></param>
        /// <returns>Uma instância do tipo RuleError</returns>
        public RuleError Add(string field, string message, params string[] args)
        {
            var text = string.Format(message, args);
            var item = new RuleError(field, text);
            Errors.Add(item);
            return item;
        }

        /// <summary>
        /// Throws a <see cref="RuleException"/> with the provided message.
        /// </summary>
        /// <param name="message">The exception message</param>
        public void Throw(string message)
        {
            throw new RuleException(message, this);
        }

        /// <summary>
        /// Check if there are errors. If yes, throws a <see cref="RuleException"/>.
        /// </summary>
        /// <param name="message">The exception message</param>
        public void Check(string message)
        {
            if (HasErrors())
                Throw(message);
        }

        /// <summary>
        /// Check if there are errors. If yes, throws a <see cref="RuleException"/>.
        /// </summary>
        public void Check()
        {
            Check(Message);
        }

        /// <summary>
        /// Merge two <see cref="RuleValidation"/> into one.
        /// </summary>
        /// <param name="source">The source object</param>
        public void Merge(RuleValidation source)
		{
			if (!source?.HasErrors() ?? false)
				return;

			if (!string.IsNullOrWhiteSpace(source.Message))
				Message = source.Message;

			Errors.AddRange(source.Errors);
		}

		#endregion
	}
}
