// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Collections;
using Prolix.Logic;
using Marketplace.Domain.Models.Trading;
using Marketplace.Logic.Queries.Trading;

namespace Marketplace.Logic.Contracts.Trading
{
    public interface IOrderService : IRepositoryService<Order>
    {
        PagedList<Order> List(OrderQuery request);
    }
}
