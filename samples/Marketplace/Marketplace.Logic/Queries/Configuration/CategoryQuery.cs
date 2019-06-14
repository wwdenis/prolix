// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Configuration;
using Prolix.Collections;

namespace Marketplace.Logic.Queries.Configuration
{
    public class CategoryQuery : QueryRequest<Category>
    {
        public CategoryQuery()
        {
            // Map sort expressions
            MapSort("Name", i => i.Name);
            MapSort("Active", i => i.Active);

            SortField = "Name";
        }

        public string Name { get; set; }
    }
}
