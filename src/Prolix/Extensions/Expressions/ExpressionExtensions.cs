// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Linq.Expressions;
using System.Reflection;

namespace Prolix.Extensions.Expressions
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Parses <see cref="<see cref="Func{T, object}" /> expressions
        /// </summary>
        /// <param name="propertyEpression">The expression to be parsed.</param>
        /// <returns>The parsed expression</returns>
        public static LambdaExpression Normalize(this LambdaExpression propertyEpression)
        {
            if (propertyEpression == null)
                return null;

            LambdaExpression lambda = propertyEpression;

            // Lambda expressions which manipulates other types than string are built over the "Convert" method
            // This algorith removes this method call to enable callin sorting by sending a generic expression
            // For Example: From: i => Convert(i.Active) | To: i => i.Active

            if (propertyEpression.Body is UnaryExpression unary)
                lambda = Expression.Lambda(unary.Operand, propertyEpression.Parameters);

            return lambda;
        }

        /// <summary>
        /// Parses <see cref="<see cref="Func{T, object}" /> expressions
        /// </summary>
        /// <param name="propertyEpression">The expression to be parsed.</param>
        /// <returns>The parsed expression</returns>
        public static PropertyInfo GetInfo(this LambdaExpression propertyEpression)
        {
            if (propertyEpression?.Body is MemberExpression expression)
                return expression.Member as PropertyInfo;

            return null;
        }
    }
}
