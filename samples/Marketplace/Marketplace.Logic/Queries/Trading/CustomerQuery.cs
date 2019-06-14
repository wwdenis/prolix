// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Trading;
using Prolix.Collections;

namespace Marketplace.Logic.Queries.Trading
{
    public class CustomerQuery : QueryRequest<Customer>
    {
        public CustomerQuery()
        {
            // Map sort expressions
            MapSort("Name", i => i.Name);
            MapSort("Email", i => i.User.Email);
            MapSort("Active", i => i.Active);

            SortField = "Name";
        }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
