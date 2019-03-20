// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Trading;
using Prolix.Core.Collections;

namespace Marketplace.Logic.Queries.Trading
{
    public class DealerQuery : QueryRequest<Dealer>
    {
        public DealerQuery()
        {
            // Map sort expressions
            MapSort("Name", i => i.Name);
            MapSort("City", i => i.City);
            MapSort("ProvinceName", i => i.Province.Name);
            MapSort("CountryName", i => i.Country.Name);
            MapSort("Active", i => i.Active);

            SortField = "Name";
        }

        public string Name { get; set; }
        public string CityName { get; set; }
        public int? ProvinceId { get; set; }
        public int? CountryId { get; set; }
        public bool HasOrders { get; set; }
    }
}
