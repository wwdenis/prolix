// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Wwa.Core.Extensions.Reflection;

namespace Wwa.Core.Collections
{
    public static class PagedListExtensions
    {
        /// <summary>
        /// Sorts a collection
        /// </summary>
        /// <typeparam name="ModelType">Model type</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="request">Sort information</param>
        /// <returns>The sorted collection.</returns>
        public static IOrderedQueryable<ModelType> ToSorted<ModelType>(this IQueryable<ModelType> source, QueryRequest<ModelType> request)
            where ModelType : class
        {
            if (source == null)
                return null;

            IOrderedQueryable<ModelType> sorted = null;

            // Sort the list based on mapped sort expressions
            if (request.SortExpression != null)
            {
                // The sort expression must be converted (and called through reflection)
                sorted = Sort(source, request.SortDescending, request.SortExpression);
            }
            else
            {
                // "Empty" sort (since a sort is required to output a sorted list)
                sorted = source.OrderBy(i => 1);
            }

            return sorted;
        }

        /// <summary>
        /// Page and sorts a collection
        /// </summary>
        /// <typeparam name="ModelType">Model type</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="request">Page information</param>
        /// <returns>The paged collection.</returns>
        public static PagedList<ModelType> ToPaged<ModelType>(this IQueryable<ModelType> source, QueryRequest<ModelType> request)
            where ModelType : class
        {
            // Sort the collection based on query information
            var query = source.ToSorted(request);

            IEnumerable<ModelType> paged = null;

            int recordCount = 0;
            int pageSize = request?.PageSize ?? 0;
            int pageNumber = request?.PageNumber ?? 1;

            if (query?.Any() ?? false)
            {
                // Returm all record if there is no page information
                if (pageSize <= 0)
                {
                    paged = query.ToList();
                }
                else
                {
                    var start = pageSize * (pageNumber - 1);
                    paged = query.Skip(start).Take(pageSize);
                }

                recordCount = query.Count();
            }

            var result = new PagedList<ModelType>(paged, recordCount, request.PageSize, request.PageNumber);

            return result;
        }

        static IOrderedQueryable<ModelType> Sort<ModelType>(IQueryable<ModelType> source, bool descending, LambdaExpression expression)
        {
            var methodName = descending ? "OrderByDescending" : "OrderBy";
            var method = typeof(Queryable).MakeGenericMethod(methodName, typeof(ModelType), expression.ReturnType);
            var args = new object[] { source, expression };
            
            var query = method.Invoke(null, args);
            var result = query as IOrderedQueryable<ModelType>;
            
            return result;
        }
    }
}
