// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Prolix.Data.EF
{

    public sealed class EFEntitySet<T> : IEntitySet<T>
        where T : class
    {
        readonly IDbSet<T> _set;
        readonly DbContext _context;

        public EFEntitySet(IDbSet<T> set, DbContext context)
        {
            _set = set ?? throw new ArgumentNullException(nameof(set));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryProvider Provider => _set.Provider;
        public Expression Expression => _set.Expression;
        public Type ElementType => _set.ElementType;
        
        public T Add(T model)
        {
            var entry = _context.Entry(model);

            if (entry.State != EntityState.Detached)
                throw new ArgumentOutOfRangeException(nameof(model), "Model was already added in the collection");

            return _set.Add(model);
        }

        public void Remove(T model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var entry = _context.Entry(model);

            if (entry.State == EntityState.Detached)
                _set.Attach(model);

            _set.Remove(model);
        }

        public void Update(T source, T target)
        {
            var entry = _context.Entry(source);

            if (entry.State == EntityState.Detached)
                throw new InvalidOperationException("Model does not exists in the collection");

            entry.CurrentValues.SetValues(target);
        }

        public bool IsSaved(T model)
        {
            var entry = _context.Entry(model);
            bool isSaved = (entry.State != EntityState.Detached);
            return isSaved;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _set.GetEnumerator();
        }
    }
}
