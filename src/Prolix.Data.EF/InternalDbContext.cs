// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prolix.Core.Data;

namespace Prolix.Data.EF
{
    internal class InternalDbContext : DbContext
    {
        readonly Type _senderType;

        public InternalDbContext(string nameOrConnectionString, IDbContext sender) : base(nameOrConnectionString)
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;

            _senderType = sender?.GetType() ?? throw new ArgumentNullException(nameof(sender));

            DisableInitializer();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(_senderType.Assembly);
            base.OnModelCreating(modelBuilder);
        }

        void DisableInitializer()
        {
            var baseMethod = typeof(Database).GetMethod("SetInitializer");
            var method = baseMethod.MakeGenericMethod(GetType());
            method?.Invoke(null, new object[] { null });
        }
    }
}
