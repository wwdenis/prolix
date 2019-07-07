// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using AutoMapper;
using System.Collections.Generic;

using Prolix.Collections;
using Prolix.Domain;

namespace Prolix.AspNet.Extensions
{
    public static class MapperExtensions
    {
        #region Public Methods

        /// <summary>
        /// Maps two models using AutoMapper.
        /// Both types must be mapped.
        /// </summary>
        public static T Map<T>(this object source)
            where T : class, new()
        {
            if (source == null)
                return null;

            return Mapper.Map<T>(source);
        }

        /// <summary>
        /// Maps two models using AutoMapper.
        /// Both types must be mapped.
        /// </summary>
        public static T Map<T>(this IIdentifiable source, int id)
            where T : class, IIdentifiable, new()
        {
            if (source == null)
                return null;

            var result = source.Map<T>();

            if (result == null)
                return null;

            result.Id = id;

            return result;
        }

        /// <summary>
        /// Maps two models using AutoMapper.
        /// Both types must be mapped.
        /// </summary>
        public static IEnumerable<TT> Map<TS, TT>(this IEnumerable<TS> source)
            where TT : class, new()
            where TS : class, new()
        {
            return Mapper.Map<IEnumerable<TT>>(source);
        }

        /// <summary>
        /// Maps two models using AutoMapper.
        /// Both types must be mapped.
        /// </summary>
        public static PagedList<TT> Map<TS, TT>(this PagedList<TS> source)
            where TT : class, new()
            where TS : class, new()
        {
            var list = Mapper.Map<IEnumerable<TT>>(source.Items);

            return new PagedList<TT>(list, source);
        }

        #endregion
    }
}
