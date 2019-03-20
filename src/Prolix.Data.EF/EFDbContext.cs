// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

using Prolix.Core.Data;

namespace Prolix.Data.EF
{
    /// <summary>
    /// Entity Framework implementation of IDbContext
    /// </summary>
    public abstract class EFDbContext : IDbContext
    {
        #region Fields

        readonly DbContext _db;
        DbContextTransaction _transaction;

        #endregion

        #region Events

        public EventHandler<string> Log;

        #endregion

        #region Constructors

        public EFDbContext(string nameOrConnectionString) 
        {
            _db = new InternalDbContext(nameOrConnectionString, this);
            _db.Database.Log = LogSql;
        }

        #endregion

        #region Protected Methods

        public IEntitySet<ModelType> Set<ModelType>()
            where ModelType : class
        {
            var set = _db.Set<ModelType>();
            return new EntitySet<ModelType>(set, _db);
        }

        async public Task<int> SaveChanges()
        {
            return await SaveChanges(CancellationToken.None);
        }

        async public Task<int> SaveChanges(CancellationToken cancellationToken)
        {
            return await _db.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Start()
        {
            if (_transaction != null)
                throw new InvalidOperationException("Nested transactions are not allowed");

            _transaction = _db.Database.BeginTransaction();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;
        }

        public void Commit()
        {
            if (_transaction == null)
                throw new InvalidOperationException("There's no active transaction");

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        #endregion

        #region Private Methods

        void LogSql(string sql)
        {
            Log?.Invoke(this, sql);
        }

        #endregion
    }
}
