// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Data;
using Marketplace.Domain.Models.Configuration;
using Marketplace.Domain.Models.Trading;
using Marketplace.Domain.Models.Geography;
using Marketplace.Domain.Models.Security;

namespace Marketplace.Data
{
    public interface IDataContext : IDbContext
    {
        IEntitySet<Category> Categories { get; }
        IEntitySet<Setting> Settings { get; }
        IEntitySet<StatusType> StatusTypes { get; }

        IEntitySet<Country> Countries { get; }
        IEntitySet<Province> Provinces { get; }

        IEntitySet<User> Users { get; }
        IEntitySet<Role> Roles { get; }

        IEntitySet<Customer> Customers { get; }
        IEntitySet<Dealer> Dealers { get; }
        IEntitySet<Order> Orders { get; }
        IEntitySet<OrderItem> OrderItems { get; }
        IEntitySet<Product> Products { get; }
    }
}
