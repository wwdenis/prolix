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
        public static TargetType Map<TargetType>(this object source)
            where TargetType : class, new()
        {
            if (source == null)
                return null;

            return Mapper.Map<TargetType>(source);
        }

        /// <summary>
        /// Maps two models using AutoMapper.
        /// Both types must be mapped.
        /// </summary>
        public static TargetType Map<TargetType>(this IIdentifiable source, int id)
            where TargetType : class, IIdentifiable, new()
        {
            if (source == null)
                return null;

            var result = source.Map<TargetType>();

            if (result == null)
                return null;

            result.Id = id;

            return result;
        }

        /// <summary>
        /// Maps two models using AutoMapper.
        /// Both types must be mapped.
        /// </summary>
        public static IEnumerable<TargetType> Map<SourceType, TargetType>(this IEnumerable<SourceType> source)
            where TargetType : class, new()
            where SourceType : class, new()
        {
            return Mapper.Map<IEnumerable<TargetType>>(source);
        }

        /// <summary>
        /// Maps two models using AutoMapper.
        /// Both types must be mapped.
        /// </summary>
        public static PagedList<TargetType> Map<SourceType, TargetType>(this PagedList<SourceType> source)
            where TargetType : class, new()
            where SourceType : class, new()
        {
            var list = Mapper.Map<IEnumerable<TargetType>>(source.Items);

            return new PagedList<TargetType>(list, source);
        }

        #endregion
    }
}
