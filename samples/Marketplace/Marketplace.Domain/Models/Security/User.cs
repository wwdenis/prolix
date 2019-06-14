// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Domain;

namespace Marketplace.Domain.Models.Security
{
    public class User : ActiveNamedModel
    {
        public string Email { get; set; }
        public int? RoleId { get; set; }
		public string IdentityId { get; set; }
        public virtual Role Role { get; set; }
	}
}
