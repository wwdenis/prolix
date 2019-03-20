// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;

namespace Prolix.Core.Logic
{
    /// <summary>
    /// Business validation exception.
    /// </summary>
    public sealed class RuleException : Exception
    {
        /// <summary>
        /// The <see cref="RuleValidation" /> instance
        /// </summary>
        public RuleValidation Rule { get; }

        public RuleException() : this(new RuleValidation())
        {
        }

        public RuleException(string message) : this(message, new RuleValidation())
        {
        }

        public RuleException(string message, Exception innerException) : this(message, new RuleValidation(), innerException)
        {
        }

        public RuleException(string message, RuleValidation rule) : this(message, rule, null)
        {
        }

        public RuleException(RuleValidation rule) : this(rule.Message, rule)
        {
        }

        public RuleException(string message, RuleValidation rule, Exception innerException) : base(message, innerException)
        {
            Rule = rule ?? throw new ArgumentNullException("rule"); ;

            if (!string.IsNullOrWhiteSpace(message))
                Rule.Message = message;
        }
    }
}
