// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using AutoMapper;
using System.Collections.Generic;

using Prolix.Core.Collections;
using Prolix.Core.Domain;

namespace Prolix.AspNet.Extensions
{
    public static class MapperExtensions
    {
        #region Public Methods

        /// <summary>
        /// Maps two models using AutoMapper.
        /// Both types must be mapped.
        /// </summary>
        public static DestinationType Map<DestinationType>(this object source)
            where DestinationType : class, new()
        {
            if (source == null)
                return null;

            // Realiza o mapeamento
            return Mapper.Map<DestinationType>(source);
        }

        /// <summary>
        /// Maps two models using AutoMapper.
        /// Both types must be mapped.
        /// </summary>
        public static DestinationType Map<DestinationType>(this IIdentifiable source, int id)
            where DestinationType : class, IIdentifiable, new()
        {
            if (source == null)
                return null;

            // Realiza o mapeamento
            var result = source.Map<DestinationType>();

            if (result == null)
                return null;

            // Associa o ID, se for informado
            result.Id = id;

            return result;
        }

        /// <summary>
        /// Maps two models using AutoMapper.
        /// Both types must be mapped.
        /// </summary>
        public static IEnumerable<DestinationType> Map<SourceType, DestinationType>(this IEnumerable<SourceType> source)
            where DestinationType : class, new()
            where SourceType : class, new()
        {
            // Realiza o mapeamento
            return Mapper.Map<IEnumerable<DestinationType>>(source);
        }

        /// <summary>
        /// Maps two models using AutoMapper.
        /// Both types must be mapped.
        /// </summary>
        public static PagedList<DestinationType> Map<SourceType, DestinationType>(this PagedList<SourceType> source)
            where DestinationType : class, new()
            where SourceType : class, new()
        {
            // Mapea a lista paginada
            var list = Mapper.Map<IEnumerable<DestinationType>>(source.Items);

            // Remonta a lista paginada com a view model
            return new PagedList<DestinationType>(list, source);
        }

        #endregion
    }
}
