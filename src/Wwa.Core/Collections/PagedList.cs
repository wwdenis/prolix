// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Wwa.Core.Collections
{
    /// <summary>
    /// Manages a page list of items
    /// </summary>
    public sealed class PagedList<ModelType> : IPageInfo
        where ModelType : class
    {
        #region Fields

        readonly static IEnumerable<ModelType> EmptyList = Enumerable.Empty<ModelType>();

        #endregion

        #region Constructors

        public PagedList(IEnumerable<ModelType> source, int recordCount, int pageSize, int pageNumber)
        {
            Items = source ?? EmptyList;
            RecordCount = recordCount;
            PageSize = pageSize;
            PageCount = ParseCount(recordCount, pageSize);
            PageNumber = ParseNumber(pageNumber, PageCount);
        }

        public PagedList(IEnumerable<ModelType> source, IPageInfo info) : this(source, info?.RecordCount ?? 0, info?.PageSize ?? 0, info?.PageNumber ?? 0)
        {
        }

        public PagedList(IEnumerable<ModelType> source) : this(source, 0, 0, 1)
        {
        }

        #endregion

        #region Properties

        public IEnumerable<ModelType> Items { get; }

        public int PageSize { get; set; }

        public int RecordCount { get; set; }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        #endregion

        #region Static Methods

        public static PagedList<ModelType> Empty()
        {
            return new PagedList<ModelType>(EmptyList);
        }

        static int ParseCount(int recordCount, int pageSize)
        {
            if (pageSize == 0 || recordCount == 0)
                return 0;

            var remainder = recordCount % pageSize;
            int count = recordCount / pageSize;

            return remainder > 0 ? ++count : count;
        }

        static int ParseNumber(int pageNumber, int pageCount)
        {
            if (pageNumber <= 0)
                return 1;

            if (pageNumber > pageCount)
                return pageCount;

            return pageNumber;
        }
        
        #endregion
    }
}
