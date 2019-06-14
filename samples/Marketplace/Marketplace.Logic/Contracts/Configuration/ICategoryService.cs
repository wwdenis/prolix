// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Collections;
using Prolix.Logic;
using Marketplace.Domain.Models.Configuration;
using Marketplace.Logic.Queries.Configuration;

namespace Marketplace.Logic.Contracts.Configuration
{
    public interface ICategoryService : IRepositoryService<Category>
    {
        PagedList<Category> List(CategoryQuery request);
    }
}
