// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Wwa.Core.Collections;
using Wwa.Core.Logic;
using Marketplace.Domain.Models.Trading;
using Marketplace.Logic.Queries.Trading;

namespace Marketplace.Logic.Contracts.Trading
{
    public interface IProductService : IUpdatableService<Product>
    {
        PagedList<Product> List(ProductQuery request);
    }
}