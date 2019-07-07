// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Prolix.Extensions.Expressions;
using Prolix.Extensions.Reflection;

namespace Prolix.Collections
{
    /// <summary>
    /// Handle paging ans sorting parameters
    /// </summary>
    /// <typeparam name="T">The model to be paged</typeparam>
    public abstract class QueryRequest<T> : IPageRequest, ISortRequest
        where T : class
    {
        #region Properties

        Dictionary<string, LambdaExpression> SortMappings { get; } = new Dictionary<string, LambdaExpression>();

        /// <summary>
        /// The page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// The page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The field to be sorted
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// The field sort direction
        /// </summary>
        public bool SortDescending { get; set; }

        /// <summary>
        /// Output type parameter (e.g. list of strings)
        /// </summary>
        public bool IsSimple { get; set; }

        /// <summary>
        /// The expression used in the sort method
        /// </summary>
        internal LambdaExpression SortExpression
        {
            get
            {
                if (SortMappings == null || string.IsNullOrWhiteSpace(SortField) || !SortMappings.ContainsKey(SortField))
                    return null;

                return SortMappings[SortField];
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Map a external field (name) to a property (expression)
        /// </summary>
        /// <param name="fieldKey">The field id sent by the api client (e.g. CityName)</param>
        /// <param name="fieldExpression">The sort delegate (Lambda expression)</param>
        protected void MapSort(string fieldKey, Expression<Func<T, object>> fieldExpression)
        {
            // Hack EF object expressions
            LambdaExpression lambda = fieldExpression.Normalize();

            if (!SortMappings.ContainsKey(fieldKey))
                SortMappings.Add(fieldKey, lambda);
        }

        #endregion
    }
}
