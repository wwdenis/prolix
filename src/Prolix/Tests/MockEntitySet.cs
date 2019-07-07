// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Prolix.Data;
using Prolix.Extensions.Reflection;

namespace Prolix.Tests
{
    /// <summary>
    /// List-based mock entity set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MockEntitySet<T> : IEntitySet<T>
        where T : class
    {
        static List<T> _list = new List<T>();

        public Expression Expression => _list.AsQueryable().Expression;
        public Type ElementType => _list.AsQueryable().ElementType;
        public IQueryProvider Provider => _list.AsQueryable().Provider;

        public MockEntitySet()
        {
        }


        public MockEntitySet(IEnumerable<T> source)
        {
            _list = new List<T>(source);
        }

        public T Add(T entity)
        {
            _list.Add(entity);
            return entity;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public bool IsSaved(T model)
        {
            return _list.Contains(model);
        }

        public void Remove(T entity)
        {
            _list.Remove(entity);
        }

        public void Update(T source, T target)
        {
            var pos = _list.IndexOf(target);

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
