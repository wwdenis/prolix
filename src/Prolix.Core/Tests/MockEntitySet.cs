// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Prolix.Core.Data;
using Prolix.Core.Extensions.Reflection;

namespace Prolix.Core.Tests
{
    /// <summary>
    /// List-based mock entity set
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public class MockEntitySet<ModelType> : IEntitySet<ModelType>
        where ModelType : class
    {
        static List<ModelType> _list = new List<ModelType>();

        public Expression Expression => _list.AsQueryable().Expression;
        public Type ElementType => _list.AsQueryable().ElementType;
        public IQueryProvider Provider => _list.AsQueryable().Provider;

        public MockEntitySet()
        {
        }


        public MockEntitySet(IEnumerable<ModelType> source)
        {
            _list = new List<ModelType>(source);
        }

        public ModelType Add(ModelType entity)
        {
            _list.Add(entity);
            return entity;
        }

        public IEnumerator<ModelType> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public bool IsSaved(ModelType model)
        {
            return _list.Contains(model);
        }

        public void Remove(ModelType entity)
        {
            _list.Remove(entity);
        }

        public void Update(ModelType source, ModelType destination)
        {
            var pos = _list.IndexOf(destination);

            if (pos < 0)
                return;

            var existing = _list.ElementAt(pos);
            existing.CopyValues(source);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
