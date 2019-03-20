// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Core.Collections;
using Prolix.Core.Logic;
using Marketplace.Domain.Models.Trading;
using Marketplace.Logic.Queries.Trading;

namespace Marketplace.Logic.Contracts.Trading
{
    public interface ICustomerService : IUpdatableService<Customer>
    {
        PagedList<Customer> List(CustomerQuery request);
    }
}
