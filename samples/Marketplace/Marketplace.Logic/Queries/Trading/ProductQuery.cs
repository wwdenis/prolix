// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Trading;
using Prolix.Collections;

namespace Marketplace.Logic.Queries.Trading
{
    public class ProductQuery : QueryRequest<Product>
    {
        public ProductQuery()
        {
            // Map sort expressions
            MapSort("Name", i => i.Name);
            MapSort("CategoryName", i => i.Category.Name);
            MapSort("DealerName", i => i.Dealer.Name);
            MapSort("Active", i => i.Active);

            SortField = "Name";
        }

        public string Name { get; set; }

        public int? CategoryId { get; set; }

        public int? DealerId { get; set; }
    }
}
