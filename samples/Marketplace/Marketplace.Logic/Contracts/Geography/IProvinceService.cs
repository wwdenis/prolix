// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Logic.Queries.Geography;
using Marketplace.Domain.Models.Geography;
using Prolix.Core.Logic;
using Prolix.Core.Collections;

namespace Marketplace.Logic.Contracts.Geography
{
    public interface IProvinceService : IRepositoryService<Province>
    {
        PagedList<Province> List(ProvinceQuery request);
    }
}
