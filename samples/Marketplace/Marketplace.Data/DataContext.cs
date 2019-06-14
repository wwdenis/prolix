// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Diagnostics;

using Prolix.Data;
using Prolix.Data.EF;

using Marketplace.Domain.Models.Configuration;
using Marketplace.Domain.Models.Trading;
using Marketplace.Domain.Models.Security;
using Marketplace.Domain.Models.Geography;

namespace Marketplace.Data
{
    public sealed class DataContext : EFDbContext, IDataContext
    {
        #region Constructors

        public DataContext() : base("DefaultConnection")
        {
            if (Debugger.IsAttached)
                Log += OnLog;
        }

        ~DataContext()
        {
            Log -= OnLog;
        }

        #endregion

        #region Event Handlers

        void OnLog(object sender, string sql) => Debug.WriteLine(sql);

        #endregion

        #region Properties

        public IEntitySet<Category> Categories => Set<Category>();
        public IEntitySet<Setting> Settings => Set<Setting>();
        public IEntitySet<StatusType> StatusTypes => Set<StatusType>();

        public IEntitySet<Country> Countries => Set<Country>();
        public IEntitySet<Province> Provinces => Set<Province>();

        public IEntitySet<User> Users => Set<User>();
        public IEntitySet<Role> Roles => Set<Role>();

        public IEntitySet<Customer> Customers => Set<Customer>();
        public IEntitySet<Dealer> Dealers => Set<Dealer>();
        public IEntitySet<Order> Orders => Set<Order>();
        public IEntitySet<OrderItem> OrderItems => Set<OrderItem>();
        public IEntitySet<Product> Products => Set<Product>();

        #endregion
    }
}
