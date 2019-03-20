// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Security;
using Prolix.Core.Collections;

namespace Marketplace.Logic.Queries.Security
{
    public class UserQuery : QueryRequest<User>
    {
        public UserQuery()
        {
            // Map sort expressions
            MapSort("Name", i => i.Name);
            MapSort("RoleName", i => i.Role.Name);
			MapSort("Email", i => i.Email);
			MapSort("Active", i => i.Active);

            SortField = "Name";
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public int? RoleId { get; set; }
    }
}
