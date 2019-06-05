// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Prolix.Core.Data;

namespace Prolix.Data.EF
{

    public sealed class EntitySet<ModelType> : IEntitySet<ModelType>
        where ModelType : class
    {
        readonly IDbSet<ModelType> _set;
        readonly DbContext _context;

        public EntitySet(IDbSet<ModelType> set, DbContext context)
        {
            _set = set ?? throw new ArgumentNullException(nameof(set));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryProvider Provider => _set.Provider;
        public Expression Expression => _set.Expression;
        public Type ElementType => _set.ElementType;
        
        public ModelType Add(ModelType model)
        {
            var entry = _context.Entry(model);

            if (entry.State != EntityState.Detached)
                throw new ArgumentOutOfRangeException("Entity was alreaded added in the collection");

            return _set.Add(model);
        }

        public void Remove(ModelType model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var entry = _context.Entry(model);

            if (entry.State == EntityState.Detached)
                _set.Attach(model);

            _set.Remove(model);
        }

        public void Update(ModelType source, ModelType destination)
        {
            var entry = _context.Entry(source);

            if (entry.State == EntityState.Detached)
                throw new InvalidOperationException("Entity does not exists in the collection");

            entry.CurrentValues.SetValues(destination);
        }

        public bool IsSaved(ModelType model)
        {
            var entry = _context.Entry(model);
            bool isSaved = (entry.State != EntityState.Detached);
            return isSaved;
        }

        public IEnumerator<ModelType> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _set.GetEnumerator();
        }
    }
}
