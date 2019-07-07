// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

using Marketplace.Data;
using Marketplace.Domain.Models.Configuration;
using Marketplace.Domain.Models.Geography;
using Marketplace.Domain.Models.Security;
using Marketplace.Domain.Models.Trading;

using Prolix.Data;
using Prolix.Tests;

namespace Marketplace.Tests.Mock
{
    public class MockDataContext : IDataContext
    {
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

        public void Commit()
        {
            // No mock for this
        }

        public void Dispose()
        {
            // No mock for this
        }

        public void Rollback()
        {
            // No mock for this
        }

        async public Task<int> SaveChanges()
        {
            return await Task.FromResult(1);
        }

        async public Task<int> SaveChanges(CancellationToken cancellationToken)
        {
            return await Task.FromResult(1);
        }

        public IEntitySet<T> Set<T>() where T : class
        {
            return new MockEntitySet<T>();
        }

        public void Start()
        {
            // No mock for this
        }
    }
}
